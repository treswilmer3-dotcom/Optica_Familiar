using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Seguridad;
using OpticaFamiliar.Infrastructure.Data;
using OpticaFamiliar.Infrastructure.Services;

namespace OpticaFamiliar.Infrastructure.Seed;

public class SeedOptions
{
    public const string Seccion = "Seed";
    /// <summary>Contraseña inicial del usuario 'admin'. Si está vacía no se crea ningún usuario.</summary>
    public string? AdminPassword { get; set; }
    /// <summary>Contraseña inicial del usuario 'superadmin' (operador de la plataforma). Si está vacía no se crea.</summary>
    public string? SuperAdminPassword { get; set; }
}

/// <summary>Datos mínimos para operar: empresa, sucursal, roles, administrador y catálogo base. Idempotente.</summary>
public class DbSeeder
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly SeedOptions _opts;
    private readonly ILogger<DbSeeder> _log;

    public DbSeeder(AppDbContext db, IPasswordHasher hasher, IOptions<SeedOptions> opts, ILogger<DbSeeder> log)
    {
        _db = db;
        _hasher = hasher;
        _opts = opts.Value;
        _log = log;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        // Sin contexto de empresa: todas las consultas saltan el filtro y asignan la empresa de forma explícita.
        var plataforma = await AsegurarEmpresa("PLATAFORMA", "Plataforma Óptica", "PLATAFORMA", ct);
        var plataformaSuc = await AsegurarSucursal(plataforma, "PLAT", "Operación de la plataforma", ct);
        var empresa = await AsegurarEmpresa("OPTICA-FAMILIAR", "Óptica Familiar", "0000000000001", ct);
        var sucursal = await AsegurarSucursal(empresa, "MATRIZ", "Sucursal Matriz", ct, "Quito", "Pichincha");

        foreach (var (codigo, nombre, desc) in new[]
        {
            (Roles.SuperAdmin, "Super administrador", "Operador de la plataforma: gestiona empresas"),
            (Roles.Administrador, "Administrador", "Administra su empresa y todas sus sucursales"),
            (Roles.Vendedor, "Vendedor", "Clientes, órdenes de trabajo y ventas"),
            (Roles.Optometrista, "Optometrista", "Clientes, exámenes visuales y recetas")
        })
        {
            if (!await _db.Roles.AnyAsync(r => r.Codigo == codigo, ct))
                _db.Roles.Add(new Rol { Codigo = codigo, Nombre = nombre, Descripcion = desc });
        }
        await _db.SaveChangesAsync(ct);

        if (!await _db.CategoriaProductos.IgnoreQueryFilters().AnyAsync(c => c.EmpresaId == empresa.Id, ct))
        {
            _db.CategoriaProductos.AddRange(Aprovisionamiento.CategoriasBase(empresa.Id));
            await _db.SaveChangesAsync(ct);
        }

        await AsegurarUsuario(plataforma, plataformaSuc, "superadmin", Roles.SuperAdmin, "Super", "Administrador", _opts.SuperAdminPassword, ct);
        await AsegurarUsuario(empresa, sucursal, "admin", Roles.Administrador, "Administrador", "Sistema", _opts.AdminPassword, ct);
    }

    private async Task<Empresa> AsegurarEmpresa(string codigo, string nombre, string fiscal, CancellationToken ct)
    {
        var e = await _db.Empresas.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Codigo == codigo, ct);
        if (e != null) return e;
        e = new Empresa { Codigo = codigo, RazonSocial = nombre, NombreComercial = nombre, IdentificacionFiscal = fiscal };
        _db.Empresas.Add(e);
        await _db.SaveChangesAsync(ct);
        return e;
    }

    private async Task<Sucursal> AsegurarSucursal(Empresa empresa, string codigo, string nombre, CancellationToken ct,
        string? ciudad = null, string? provincia = null)
    {
        var s = await _db.Sucursales.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.EmpresaId == empresa.Id && x.Codigo == codigo, ct);
        if (s == null)
        {
            s = new Sucursal { EmpresaId = empresa.Id, Codigo = codigo, Nombre = nombre, Ciudad = ciudad, Provincia = provincia };
            _db.Sucursales.Add(s);
            await _db.SaveChangesAsync(ct);
        }
        foreach (var tipo in NumeracionDefecto.Tipos)
        {
            if (!await _db.NumeracionDocumentos.IgnoreQueryFilters().AnyAsync(n => n.SucursalId == s.Id && n.TipoDocumento == tipo, ct))
            {
                var n = NumeracionDefecto.Crear(s.Id, s.Codigo, tipo);
                n.EmpresaId = empresa.Id;
                _db.NumeracionDocumentos.Add(n);
            }
        }
        await _db.SaveChangesAsync(ct);
        return s;
    }

    private async Task AsegurarUsuario(Empresa empresa, Sucursal sucursal, string username, string rolCodigo,
        string nombres, string apellidos, string? password, CancellationToken ct)
    {
        if (await _db.Usuarios.IgnoreQueryFilters().AnyAsync(u => u.EmpresaId == empresa.Id && u.Username == username, ct))
            return;
        if (string.IsNullOrWhiteSpace(password))
        {
            _log.LogWarning("No se creó el usuario '{Usuario}' de {Empresa}: falta su contraseña inicial en Seed (user-secrets o variable de entorno).", username, empresa.Codigo);
            return;
        }
        var rol = await _db.Roles.FirstAsync(r => r.Codigo == rolCodigo, ct);
        _db.Usuarios.Add(new Usuario
        {
            EmpresaId = empresa.Id,
            Persona = new Persona { EmpresaId = empresa.Id, Nombres = nombres, Apellidos = apellidos },
            RolId = rol.Id, SucursalId = sucursal.Id, Username = username,
            PasswordHash = _hasher.Hash(password)
        });
        await _db.SaveChangesAsync(ct);
        _log.LogInformation("Usuario '{Usuario}' creado en {Empresa}.", username, empresa.Codigo);
    }
}
