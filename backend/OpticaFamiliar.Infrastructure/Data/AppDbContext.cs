using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Domain.Entities;
using OpticaFamiliar.Domain.Entities.AgendaMedica;
using OpticaFamiliar.Domain.Entities.Auditoria;
using OpticaFamiliar.Domain.Entities.Caja;
using OpticaFamiliar.Domain.Entities.Compras;
using OpticaFamiliar.Domain.Entities.Configuracion;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;
using OpticaFamiliar.Domain.Entities.Inventario;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.OrdenesTrabajo;
using OpticaFamiliar.Domain.Entities.Pagos;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Productos;
using OpticaFamiliar.Domain.Entities.Proveedores;
using OpticaFamiliar.Domain.Entities.Seguridad;
using OpticaFamiliar.Domain.Entities.Transferencias;
using OpticaFamiliar.Domain.Entities.Ventas;
using OpticaFamiliar.Domain.Entities.Common;
using UsuarioEntity = OpticaFamiliar.Domain.Entities.Personas.Usuario;

namespace OpticaFamiliar.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Módulo Organización
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<EmpresaConfiguracion> EmpresaConfiguraciones { get; set; }
    public DbSet<Sucursal> Sucursales { get; set; }
    public DbSet<SucursalConfiguracion> SucursalConfiguraciones { get; set; }

    // Módulo Seguridad
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }
    public DbSet<RolPermiso> RolPermisos { get; set; }

    // Módulo Personas
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Optometrista> Optometristas { get; set; }

    // Módulo Agenda Médica
    public DbSet<Cita> Citas { get; set; }

    // Módulo Historia Clínica
    public DbSet<HistoriaClinica> HistoriasClinicas { get; set; }
    public DbSet<Consulta> Consultas { get; set; }
    public DbSet<Receta> Recetas { get; set; }

    // Módulo Productos
    public DbSet<CategoriaProducto> CategoriaProductos { get; set; }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Producto> Productos { get; set; }

    // Módulo Inventario
    public DbSet<Inventario> Inventarios { get; set; }
    public DbSet<MovimientoInventario> MovimientosInventario { get; set; }

    // Módulo Transferencias
    public DbSet<Transferencia> Transferencias { get; set; }
    public DbSet<TransferenciaDetalle> TransferenciaDetalles { get; set; }

    // Módulo Proveedores
    public DbSet<Proveedor> Proveedores { get; set; }

    // Módulo Compras
    public DbSet<Compra> Compras { get; set; }
    public DbSet<CompraDetalle> CompraDetalles { get; set; }

    // Módulo Ventas
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentaDetalles { get; set; }

    // Módulo Órdenes de Trabajo
    public DbSet<OrdenTrabajo> OrdenesTrabajo { get; set; }

    // Módulo Caja
    public DbSet<Caja> Cajas { get; set; }
    public DbSet<MovimientoCaja> MovimientosCaja { get; set; }

    // Módulo Pagos
    public DbSet<Pago> Pagos { get; set; }

    // Módulo Configuración
    public DbSet<ConfiguracionSistema> ConfiguracionesSistema { get; set; }
    public DbSet<ParametroCatalogo> ParametroCatalogos { get; set; }
    public DbSet<NumeracionDocumento> NumeracionDocumentos { get; set; }

    // Módulo Auditoría
    public DbSet<Auditoria> Auditorias { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var ahora = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion ??= ahora;
                entry.Entity.Estado ??= "ACTIVO";
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaModificacion = ahora;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones y llaves compuestas
        modelBuilder.Entity<RolPermiso>()
            .HasKey(rp => new { rp.RolId, rp.PermisoId });

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany(r => r.RolPermisos)
            .HasForeignKey(rp => rp.RolId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany(p => p.RolPermisos)
            .HasForeignKey(rp => rp.PermisoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de relaciones Usuario -> Transferencia (dos relaciones)
        modelBuilder.Entity<Transferencia>()
            .HasOne(t => t.UsuarioSolicita)
            .WithMany(u => u.TransferenciasSolicitadas)
            .HasForeignKey(t => t.UsuarioSolicitaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transferencia>()
            .HasOne(t => t.UsuarioAprueba)
            .WithMany()
            .HasForeignKey(t => t.UsuarioApruebaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Precisión decimal (importes y dioptrías): numeric(18,2)
        foreach (var prop in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            prop.SetPrecision(18);
            prop.SetScale(2);
        }

        // Unicidad de claves de negocio
        modelBuilder.Entity<Usuario>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Rol>().HasIndex(r => r.Codigo).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(e => e.Ruc).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(e => e.Codigo).IsUnique();
        modelBuilder.Entity<Sucursal>().HasIndex(s => new { s.EmpresaId, s.Codigo }).IsUnique();
        modelBuilder.Entity<Persona>().HasIndex(p => p.NumeroIdentificacion).IsUnique()
            .HasFilter("numero_identificacion IS NOT NULL");
        modelBuilder.Entity<Producto>().HasIndex(p => p.Codigo).IsUnique();
        modelBuilder.Entity<Venta>().HasIndex(v => new { v.SucursalId, v.NumeroFactura }).IsUnique();
        modelBuilder.Entity<OrdenTrabajo>().HasIndex(o => o.NumeroOrden).IsUnique();
        modelBuilder.Entity<HistoriaClinica>().HasIndex(h => h.NumeroHistoria).IsUnique();
        modelBuilder.Entity<NumeracionDocumento>().HasIndex(n => new { n.SucursalId, n.TipoDocumento }).IsUnique();
    }
}
