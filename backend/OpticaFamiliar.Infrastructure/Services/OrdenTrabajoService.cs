using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.OrdenesTrabajo;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class OrdenTrabajoService : IOrdenTrabajoService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;
    private readonly NumeradorDocumentos _numerador;

    public OrdenTrabajoService(AppDbContext db, ICurrentUser current)
    {
        _db = db;
        _current = current;
        _numerador = new NumeradorDocumentos(db);
    }

    public async Task<OrdenTrabajoDto> CrearAsync(OrdenTrabajoRequest r, CancellationToken ct = default)
    {
        if (!await _db.Recetas.AnyAsync(x => x.Id == r.RecetaId, ct))
            throw new BusinessRuleException("La receta indicada no existe.");
        if (r.FechaEntregaEstimada.HasValue && r.FechaEntregaEstimada.Value.Date < DateTime.UtcNow.Date)
            throw new BusinessRuleException("La fecha de entrega estimada no puede ser anterior a hoy.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var orden = new OrdenTrabajo
        {
            RecetaId = r.RecetaId,
            NumeroOrden = await _numerador.SiguienteAsync(_current.SucursalId, TiposDocumento.OrdenTrabajo, "OT", ct),
            FechaIngreso = DateTime.UtcNow,
            FechaEntregaEstimada = r.FechaEntregaEstimada.HasValue
                ? DateTime.SpecifyKind(r.FechaEntregaEstimada.Value, DateTimeKind.Utc) : null,
            Estado = Estados.OrdenCreada,
            Observaciones = r.Observaciones
        };
        _db.OrdenesTrabajo.Add(orden);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return Map(orden);
    }

    public async Task<OrdenTrabajoDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.OrdenesTrabajo.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, ct)
            ?? throw new NotFoundException("Orden de trabajo no encontrada."));

    public async Task<IReadOnlyList<OrdenTrabajoDto>> ListarAsync(string? estado, CancellationToken ct = default)
    {
        var q = _db.OrdenesTrabajo.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(estado)) q = q.Where(o => o.Estado == estado.ToUpper());
        return (await q.OrderByDescending(o => o.FechaIngreso).Take(200).ToListAsync(ct)).Select(Map).ToList();
    }

    public async Task<OrdenTrabajoDto> CambiarEstadoAsync(long id, CambiarEstadoOrdenRequest r, CancellationToken ct = default)
    {
        var orden = await _db.OrdenesTrabajo.FirstOrDefaultAsync(o => o.Id == id, ct)
            ?? throw new NotFoundException("Orden de trabajo no encontrada.");

        var nuevo = r.Estado.Trim().ToUpper();
        var idxNuevo = Array.IndexOf(Estados.Ordenes, nuevo);
        if (idxNuevo < 0)
            throw new BusinessRuleException($"Estado inválido. Valores: {string.Join(", ", Estados.Ordenes)}.");
        if (idxNuevo <= Array.IndexOf(Estados.Ordenes, orden.Estado))
            throw new BusinessRuleException($"No se puede pasar de {orden.Estado} a {nuevo}.");

        orden.Estado = nuevo;
        if (nuevo == Estados.OrdenEntregada) orden.FechaEntregaReal = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return Map(orden);
    }

    internal static OrdenTrabajoDto Map(OrdenTrabajo o) => new()
    {
        Id = o.Id, VentaId = o.VentaId, RecetaId = o.RecetaId, NumeroOrden = o.NumeroOrden,
        FechaIngreso = o.FechaIngreso, FechaEntregaEstimada = o.FechaEntregaEstimada,
        FechaEntregaReal = o.FechaEntregaReal, Estado = o.Estado, Observaciones = o.Observaciones
    };
}
