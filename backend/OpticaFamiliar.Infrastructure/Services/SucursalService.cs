using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class SucursalService : ISucursalService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;

    public SucursalService(AppDbContext db, ICurrentUser current)
    {
        _db = db;
        _current = current;
    }

    /// <summary>El SUPERADMIN ve las sucursales de todas las empresas; el resto, solo las de la suya.</summary>
    private IQueryable<Sucursal> Base() => _current.EsSuperAdmin ? _db.Sucursales.IgnoreQueryFilters() : _db.Sucursales;

    public async Task<IReadOnlyList<SucursalDto>> ListarAsync(CancellationToken ct = default) =>
        (await Base().AsNoTracking().OrderBy(s => s.EmpresaId).ThenBy(s => s.Nombre).ToListAsync(ct)).Select(Map).ToList();

    public async Task<SucursalDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await Base().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new NotFoundException("Sucursal no encontrada."));

    public async Task<SucursalDto> CrearAsync(SucursalRequest r, CancellationToken ct = default)
    {
        var empresaId = _current.EsSuperAdmin
            ? r.EmpresaId ?? throw new BusinessRuleException("Indique la empresa de la sucursal.")
            : _current.EmpresaId;

        if (!await _db.Empresas.IgnoreQueryFilters().AnyAsync(e => e.Id == empresaId, ct))
            throw new BusinessRuleException("La empresa indicada no existe.");
        await ValidarCodigo(null, empresaId, r.Codigo, ct);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var s = new Sucursal { EmpresaId = empresaId };
        Aplicar(s, r);
        _db.Sucursales.Add(s);
        await _db.SaveChangesAsync(ct);

        foreach (var tipo in NumeracionDefecto.Tipos)
        {
            var n = NumeracionDefecto.Crear(s.Id, s.Codigo, tipo);
            n.EmpresaId = empresaId;
            _db.NumeracionDocumentos.Add(n);
        }
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return Map(s);
    }

    public async Task<SucursalDto> ActualizarAsync(long id, SucursalRequest r, CancellationToken ct = default)
    {
        var s = await Base().FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Sucursal no encontrada.");
        await ValidarCodigo(id, s.EmpresaId, r.Codigo, ct);
        Aplicar(s, r);
        await _db.SaveChangesAsync(ct);
        return Map(s);
    }

    private async Task ValidarCodigo(long? id, long empresaId, string codigo, CancellationToken ct)
    {
        var c = codigo.Trim();
        if (await _db.Sucursales.IgnoreQueryFilters().AnyAsync(s => s.Id != id && s.EmpresaId == empresaId && s.Codigo == c, ct))
            throw new BusinessRuleException("Ya existe una sucursal con ese código en la empresa.");
    }

    // La empresa de una sucursal no se modifica una vez creada.
    private static void Aplicar(Sucursal s, SucursalRequest r)
    {
        s.Codigo = r.Codigo.Trim();
        s.Nombre = r.Nombre.Trim();
        s.Direccion = r.Direccion;
        s.Telefono = r.Telefono;
        s.Correo = r.Correo;
        s.Ciudad = r.Ciudad;
        s.Provincia = r.Provincia;
    }

    private static SucursalDto Map(Sucursal s) => new()
    {
        Id = s.Id, EmpresaId = s.EmpresaId, Codigo = s.Codigo, Nombre = s.Nombre, Direccion = s.Direccion,
        Telefono = s.Telefono, Correo = s.Correo, Ciudad = s.Ciudad, Provincia = s.Provincia, Estado = s.Estado
    };
}
