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
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Common;
using UsuarioEntity = OpticaFamiliar.Domain.Entities.Personas.Usuario;

namespace OpticaFamiliar.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ITenantContext? _tenant;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext? tenant = null) : base(options)
    {
        _tenant = tenant;
    }

    /// <summary>Empresa del usuario autenticado. 0 = sin contexto: los filtros no devuelven nada (falla cerrado).</summary>
    private long EmpresaActual => _tenant?.EmpresaActual ?? 0;

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
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity b)
            {
                if (entry.State == EntityState.Added)
                {
                    b.FechaCreacion ??= ahora;
                    b.Estado ??= "ACTIVO";
                }
                else if (entry.State == EntityState.Modified)
                {
                    b.FechaModificacion = ahora;
                }
            }

            if (entry.Entity is IEmpresaOwned e)
                AsignarEmpresa(entry, e);
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Asigna la empresa al insertar y bloquea escrituras hacia otra empresa.</summary>
    private void AsignarEmpresa(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry, IEmpresaOwned e)
    {
        if (entry.State == EntityState.Added)
        {
            if (_tenant?.EmpresaActual is long actual && !_tenant.EsSuperAdmin)
            {
                if (e.EmpresaId == 0) e.EmpresaId = actual;
                else if (e.EmpresaId != actual)
                    throw new InvalidOperationException("Intento de escribir datos de otra empresa.");
            }
            else if (e.EmpresaId == 0 && _tenant?.EmpresaActual is long superActual)
            {
                e.EmpresaId = superActual;
            }
            if (e.EmpresaId == 0)
                throw new InvalidOperationException($"{entry.Metadata.ClrType.Name} sin empresa asignada.");
        }
        else if (entry.State == EntityState.Modified && entry.Property(nameof(IEmpresaOwned.EmpresaId)).IsModified)
        {
            throw new InvalidOperationException("No se puede cambiar la empresa de un registro.");
        }
    }

    private void AplicarFiltroEmpresa<T>(ModelBuilder mb) where T : class, IEmpresaOwned =>
        mb.Entity<T>().HasQueryFilter(x => x.EmpresaId == EmpresaActual);

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

        // --- Multiempresa: filtro global por empresa + FK a empresa en todas las entidades de la empresa ---
        var aplicar = typeof(AppDbContext).GetMethod(nameof(AplicarFiltroEmpresa),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!;
        foreach (var tipo in modelBuilder.Model.GetEntityTypes().Select(t => t.ClrType)
                     .Where(t => typeof(IEmpresaOwned).IsAssignableFrom(t)).ToList())
        {
            aplicar.MakeGenericMethod(tipo).Invoke(this, [modelBuilder]);

            // Sucursal y EmpresaConfiguracion ya tienen su relación con Empresa configurada por navegación.
            if (tipo != typeof(Sucursal) && tipo != typeof(EmpresaConfiguracion))
            {
                modelBuilder.Entity(tipo).HasOne(typeof(Empresa)).WithMany()
                    .HasForeignKey(nameof(IEmpresaOwned.EmpresaId)).OnDelete(DeleteBehavior.Restrict);
            }
        }
        modelBuilder.Entity<Empresa>().HasQueryFilter(e => e.Id == EmpresaActual);

        // Unicidad de claves de negocio (siempre dentro de la empresa)
        modelBuilder.Entity<Usuario>().HasIndex(u => new { u.EmpresaId, u.Username }).IsUnique();
        modelBuilder.Entity<Rol>().HasIndex(r => r.Codigo).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(e => e.Codigo).IsUnique();
        modelBuilder.Entity<Empresa>().HasIndex(e => new { e.Pais, e.IdentificacionFiscal }).IsUnique();
        modelBuilder.Entity<Sucursal>().HasIndex(s => new { s.EmpresaId, s.Codigo }).IsUnique();
        modelBuilder.Entity<Persona>().HasIndex(p => new { p.EmpresaId, p.NumeroIdentificacion }).IsUnique()
            .HasFilter("numero_identificacion IS NOT NULL");
        modelBuilder.Entity<Producto>().HasIndex(p => new { p.EmpresaId, p.Codigo }).IsUnique();
        modelBuilder.Entity<Venta>().HasIndex(v => new { v.SucursalId, v.NumeroFactura }).IsUnique();
        modelBuilder.Entity<OrdenTrabajo>().HasIndex(o => new { o.EmpresaId, o.NumeroOrden }).IsUnique();
        modelBuilder.Entity<HistoriaClinica>().HasIndex(h => new { h.EmpresaId, h.NumeroHistoria }).IsUnique();
        modelBuilder.Entity<NumeracionDocumento>().HasIndex(n => new { n.SucursalId, n.TipoDocumento }).IsUnique();
    }
}
