using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Configuracion;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Productos;
using OpticaFamiliar.Domain.Entities.Seguridad;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Seed;

public class SeedOptions
{
    public const string Seccion = "Seed";
    /// <summary>Contraseña inicial del usuario 'admin'. Si está vacía no se crea ningún usuario.</summary>
    public string? AdminPassword { get; set; }
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
        var empresa = await _db.Empresas.FirstOrDefaultAsync(ct);
        if (empresa == null)
        {
            empresa = new Empresa
            {
                Codigo = "OPTICA-FAMILIAR", RazonSocial = "Óptica Familiar", NombreComercial = "Óptica Familiar",
                Ruc = "0000000000001"
            };
            _db.Empresas.Add(empresa);
            await _db.SaveChangesAsync(ct);
        }

        var sucursal = await _db.Sucursales.FirstOrDefaultAsync(ct);
        if (sucursal == null)
        {
            sucursal = new Sucursal { EmpresaId = empresa.Id, Codigo = "MATRIZ", Nombre = "Sucursal Matriz" };
            _db.Sucursales.Add(sucursal);
            await _db.SaveChangesAsync(ct);
        }

        foreach (var (codigo, nombre, desc) in new[]
        {
            (Roles.Administrador, "Administrador", "Acceso total al sistema"),
            (Roles.Vendedor, "Vendedor", "Clientes, órdenes de trabajo y ventas"),
            (Roles.Optometrista, "Optometrista", "Clientes, exámenes visuales y recetas")
        })
        {
            if (!await _db.Roles.AnyAsync(r => r.Codigo == codigo, ct))
                _db.Roles.Add(new Rol { Codigo = codigo, Nombre = nombre, Descripcion = desc });
        }

        foreach (var tipo in new[] { (TiposDocumento.Venta, "001"), (TiposDocumento.OrdenTrabajo, "OT"), (TiposDocumento.HistoriaClinica, "HC") })
        {
            if (!await _db.NumeracionDocumentos.AnyAsync(n => n.SucursalId == sucursal.Id && n.TipoDocumento == tipo.Item1, ct))
                _db.NumeracionDocumentos.Add(new NumeracionDocumento
                {
                    SucursalId = sucursal.Id, TipoDocumento = tipo.Item1, Serie = tipo.Item2,
                    NumeroActual = 0, NumeroFinal = 999_999_999, Estado = Estados.Activo
                });
        }

        if (!await _db.CategoriaProductos.AnyAsync(ct))
        {
            _db.CategoriaProductos.AddRange(
                new CategoriaProducto { Codigo = "MONTURA", Nombre = "Monturas", Estado = Estados.Activo },
                new CategoriaProducto { Codigo = "LENTE", Nombre = "Lentes", Estado = Estados.Activo },
                new CategoriaProducto { Codigo = "SERVICIO", Nombre = "Servicios", Estado = Estados.Activo });
        }
        await _db.SaveChangesAsync(ct);

        if (!await _db.Usuarios.AnyAsync(u => u.Username == "admin", ct))
        {
            if (string.IsNullOrWhiteSpace(_opts.AdminPassword))
            {
                _log.LogWarning("No se creó el usuario 'admin': configure Seed:AdminPassword (user-secrets o variable de entorno).");
                return;
            }
            var rolAdmin = await _db.Roles.FirstAsync(r => r.Codigo == Roles.Administrador, ct);
            _db.Usuarios.Add(new Usuario
            {
                Persona = new Persona { Nombres = "Administrador", Apellidos = "Sistema" },
                RolId = rolAdmin.Id, SucursalId = sucursal.Id, Username = "admin",
                PasswordHash = _hasher.Hash(_opts.AdminPassword)
            });
            await _db.SaveChangesAsync(ct);
            _log.LogInformation("Usuario 'admin' creado.");
        }
    }
}
