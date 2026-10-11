using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;
    private readonly IExamenVisualService _examenes;
    private readonly IOrdenTrabajoService _ordenes;
    private readonly IVentaService _ventas;

    public ClienteService(AppDbContext db, IExamenVisualService examenes, IOrdenTrabajoService ordenes, IVentaService ventas)
    {
        _db = db;
        _examenes = examenes;
        _ordenes = ordenes;
        _ventas = ventas;
    }

    private IQueryable<Cliente> Activos() =>
        _db.Clientes.Include(c => c.Persona).Where(c => c.Persona.Estado == Estados.Activo);

    public async Task<IReadOnlyList<ClienteDto>> ListarAsync(string? buscar, int pagina, int tamano, CancellationToken ct = default)
    {
        pagina = Math.Max(pagina, 1);
        tamano = Math.Clamp(tamano, 1, 100);

        var q = Activos().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var t = buscar.Trim().ToLower();
            q = q.Where(c => (c.Persona.Nombres ?? "").ToLower().Contains(t)
                          || (c.Persona.Apellidos ?? "").ToLower().Contains(t)
                          || (c.Persona.NumeroIdentificacion ?? "").Contains(t));
        }

        var lista = await q.OrderBy(c => c.Persona.Apellidos).ThenBy(c => c.Persona.Nombres)
            .Skip((pagina - 1) * tamano).Take(tamano).ToListAsync(ct);
        return lista.Select(Map).ToList();
    }

    public async Task<ClienteDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await Activos().AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException("Cliente no encontrado."));

    public async Task<ClienteDto> CrearAsync(ClienteRequest r, CancellationToken ct = default)
    {
        var ident = r.NumeroIdentificacion.Trim();
        if (await _db.Personas.AnyAsync(p => p.NumeroIdentificacion == ident, ct))
            throw new BusinessRuleException("Ya existe una persona registrada con esa identificación.");

        var cliente = new Cliente { Persona = new Persona(), FechaRegistro = DateTime.UtcNow };
        Aplicar(cliente, r);
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync(ct);
        return Map(cliente);
    }

    public async Task<ClienteDto> ActualizarAsync(long id, ClienteRequest r, CancellationToken ct = default)
    {
        var cliente = await Activos().FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException("Cliente no encontrado.");
        var ident = r.NumeroIdentificacion.Trim();
        if (await _db.Personas.AnyAsync(p => p.Id != cliente.PersonaId && p.NumeroIdentificacion == ident, ct))
            throw new BusinessRuleException("Ya existe otra persona con esa identificación.");

        Aplicar(cliente, r);
        await _db.SaveChangesAsync(ct);
        return Map(cliente);
    }

    /// <summary>Baja lógica: se conserva el historial clínico y comercial.</summary>
    public async Task EliminarAsync(long id, CancellationToken ct = default)
    {
        var cliente = await Activos().FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new NotFoundException("Cliente no encontrado.");
        cliente.Persona.Estado = Estados.Inactivo;
        await _db.SaveChangesAsync(ct);
    }

    public async Task<HistorialClienteDto> ObtenerHistorialAsync(long id, CancellationToken ct = default)
    {
        var cliente = await ObtenerAsync(id, ct);

        var examenes = await _examenes.ListarPorClienteAsync(id, ct);

        var recetaIds = examenes.Where(e => e.Receta != null).Select(e => e.Receta!.Id).ToList();
        var ordenes = await _db.OrdenesTrabajo.AsNoTracking()
            .Where(o => (o.RecetaId != null && recetaIds.Contains(o.RecetaId.Value))
                     || (o.Venta != null && o.Venta.ClienteId == id))
            .OrderByDescending(o => o.FechaIngreso).ToListAsync(ct);

        // Reutiliza la regla de visibilidad por sucursal del servicio de ventas.
        var ventas = new List<VentaDto>();
        var ids = await _db.Ventas.AsNoTracking().Where(v => v.ClienteId == id)
            .OrderByDescending(v => v.FechaVenta).Select(v => v.Id).ToListAsync(ct);
        foreach (var vid in ids)
        {
            try { ventas.Add(await _ventas.ObtenerAsync(vid, ct)); }
            catch (ForbiddenException) { /* venta de otra sucursal: no visible para este usuario */ }
        }

        return new HistorialClienteDto
        {
            Cliente = cliente,
            Examenes = examenes.ToList(),
            Ordenes = ordenes.Select(OrdenTrabajoService.Map).ToList(),
            Ventas = ventas
        };
    }

    private static void Aplicar(Cliente c, ClienteRequest r)
    {
        c.Persona.TipoIdentificacion = r.TipoIdentificacion;
        c.Persona.NumeroIdentificacion = r.NumeroIdentificacion.Trim();
        c.Persona.Nombres = r.Nombres.Trim();
        c.Persona.Apellidos = r.Apellidos.Trim();
        c.Persona.FechaNacimiento = r.FechaNacimiento.HasValue
            ? DateTime.SpecifyKind(r.FechaNacimiento.Value, DateTimeKind.Utc) : null;
        c.Persona.Genero = r.Genero;
        c.Persona.Telefono = r.Telefono;
        c.Persona.Celular = r.Celular;
        c.Persona.Correo = r.Correo;
        c.Persona.Direccion = r.Direccion;
        c.Observaciones = r.Observaciones;
    }

    private static ClienteDto Map(Cliente c) => new()
    {
        Id = c.Id, PersonaId = c.PersonaId, FechaRegistro = c.FechaRegistro, Observaciones = c.Observaciones,
        TipoIdentificacion = c.Persona.TipoIdentificacion, NumeroIdentificacion = c.Persona.NumeroIdentificacion ?? "",
        Nombres = c.Persona.Nombres ?? "", Apellidos = c.Persona.Apellidos ?? "",
        FechaNacimiento = c.Persona.FechaNacimiento, Genero = c.Persona.Genero, Telefono = c.Persona.Telefono,
        Celular = c.Persona.Celular, Correo = c.Persona.Correo, Direccion = c.Persona.Direccion
    };
}
