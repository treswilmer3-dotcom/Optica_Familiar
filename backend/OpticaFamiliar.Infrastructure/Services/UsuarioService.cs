using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly ICurrentUser _current;

    public UsuarioService(AppDbContext db, IPasswordHasher hasher, ICurrentUser current)
    {
        _db = db;
        _hasher = hasher;
        _current = current;
    }

    /// <summary>El SUPERADMIN gestiona usuarios de todas las empresas; el resto, solo los de la suya.</summary>
    private IQueryable<Usuario> Consulta() =>
        (_current.EsSuperAdmin ? _db.Usuarios.IgnoreQueryFilters() : _db.Usuarios)
            .Include(u => u.Rol).Include(u => u.Sucursal).Include(u => u.Persona);

    public async Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken ct = default) =>
        (await Consulta().AsNoTracking().OrderBy(u => u.Username).ToListAsync(ct)).Select(Map).ToList();

    public async Task<UsuarioDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await Consulta().AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct)
            ?? throw new NotFoundException("Usuario no encontrado."));

    public async Task<UsuarioDto> CrearAsync(UsuarioCreateRequest r, CancellationToken ct = default)
    {
        var empresaId = _current.EsSuperAdmin ? r.EmpresaId ?? _current.EmpresaId : _current.EmpresaId;
        var username = r.Username.Trim();

        var rol = await _db.Roles.FirstOrDefaultAsync(x => x.Id == r.RolId, ct)
            ?? throw new BusinessRuleException("El rol indicado no existe.");
        if (rol.Codigo == Roles.SuperAdmin && !_current.EsSuperAdmin)
            throw new ForbiddenException("No puede asignar el rol SUPERADMIN.");
        if (!await _db.Empresas.IgnoreQueryFilters().AnyAsync(e => e.Id == empresaId, ct))
            throw new BusinessRuleException("La empresa indicada no existe.");
        if (!await _db.Sucursales.IgnoreQueryFilters().AnyAsync(s => s.Id == r.SucursalId && s.EmpresaId == empresaId, ct))
            throw new BusinessRuleException("La sucursal indicada no existe en la empresa.");
        if (await _db.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.EmpresaId == empresaId && u.Username == username, ct))
            throw new BusinessRuleException("El nombre de usuario ya existe en la empresa.");
        if (!string.IsNullOrWhiteSpace(r.NumeroIdentificacion) && await _db.Personas.IgnoreQueryFilters()
                .AnyAsync(p => p.EmpresaId == empresaId && p.NumeroIdentificacion == r.NumeroIdentificacion, ct))
            throw new BusinessRuleException("Ya existe una persona registrada con esa identificación.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var persona = new Persona
        {
            EmpresaId = empresaId,
            TipoIdentificacion = r.TipoIdentificacion,
            NumeroIdentificacion = string.IsNullOrWhiteSpace(r.NumeroIdentificacion) ? null : r.NumeroIdentificacion.Trim(),
            Nombres = r.Nombres.Trim(),
            Apellidos = r.Apellidos.Trim(),
            Correo = r.Correo,
            Celular = r.Celular
        };
        var usuario = new Usuario
        {
            EmpresaId = empresaId,
            Persona = persona,
            RolId = r.RolId,
            SucursalId = r.SucursalId,
            Username = username,
            PasswordHash = _hasher.Hash(r.Password)
        };
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync(ct);

        if (rol.Codigo == Roles.Optometrista)
        {
            _db.Optometristas.Add(new Optometrista
            {
                EmpresaId = empresaId, PersonaId = persona.Id, UsuarioId = usuario.Id, Estado = Estados.Activo
            });
            await _db.SaveChangesAsync(ct);
        }

        await tx.CommitAsync(ct);
        return await ObtenerAsync(usuario.Id, ct);
    }

    public async Task<UsuarioDto> ActualizarAsync(long id, UsuarioUpdateRequest r, CancellationToken ct = default)
    {
        var u = await (_current.EsSuperAdmin ? _db.Usuarios.IgnoreQueryFilters() : _db.Usuarios)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Usuario no encontrado.");
        if (r.Estado != Estados.Activo && r.Estado != Estados.Inactivo)
            throw new BusinessRuleException("Estado inválido (ACTIVO / INACTIVO).");
        if (id == _current.UserId && r.Estado != Estados.Activo)
            throw new BusinessRuleException("No puede desactivar su propio usuario.");
        var rol = await _db.Roles.FirstOrDefaultAsync(x => x.Id == r.RolId, ct)
            ?? throw new BusinessRuleException("El rol indicado no existe.");
        if (rol.Codigo == Roles.SuperAdmin && !_current.EsSuperAdmin)
            throw new ForbiddenException("No puede asignar el rol SUPERADMIN.");
        if (!await _db.Sucursales.IgnoreQueryFilters().AnyAsync(s => s.Id == r.SucursalId && s.EmpresaId == u.EmpresaId, ct))
            throw new BusinessRuleException("La sucursal indicada no existe en la empresa.");

        u.RolId = r.RolId;
        u.SucursalId = r.SucursalId;
        u.Estado = r.Estado;
        await _db.SaveChangesAsync(ct);
        return await ObtenerAsync(id, ct);
    }

    public async Task CambiarPasswordAsync(long id, CambiarPasswordRequest r, CancellationToken ct = default)
    {
        if (id != _current.UserId && !_current.EsAdministrador && !_current.EsSuperAdmin)
            throw new ForbiddenException("Solo puede cambiar su propia contraseña.");

        var u = await (_current.EsSuperAdmin ? _db.Usuarios.IgnoreQueryFilters() : _db.Usuarios)
            .FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Usuario no encontrado.");

        // Un administrador puede restablecer la de otro usuario; el propio usuario debe confirmar la actual.
        if (id == _current.UserId &&
            (string.IsNullOrEmpty(r.PasswordActual) || !_hasher.Verify(r.PasswordActual, u.PasswordHash)))
            throw new BusinessRuleException("La contraseña actual no es correcta.");

        u.PasswordHash = _hasher.Hash(r.PasswordNueva);
        await _db.SaveChangesAsync(ct);
    }

    private static UsuarioDto Map(Usuario u) => new()
    {
        Id = u.Id, EmpresaId = u.EmpresaId, Username = u.Username, RolId = u.RolId, Rol = u.Rol.Codigo, SucursalId = u.SucursalId,
        Sucursal = u.Sucursal?.Nombre,
        NombreCompleto = u.Persona == null ? null : $"{u.Persona.Nombres} {u.Persona.Apellidos}".Trim(),
        Estado = u.Estado, UltimoAcceso = u.UltimoAcceso
    };
}
