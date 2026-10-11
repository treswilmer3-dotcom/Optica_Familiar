using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class RecetaService : IRecetaService
{
    private readonly AppDbContext _db;
    public RecetaService(AppDbContext db) => _db = db;

    public async Task<RecetaDto> CrearAsync(RecetaRequest r, CancellationToken ct = default)
    {
        if (!await _db.Consultas.AnyAsync(c => c.Id == r.ConsultaId, ct))
            throw new BusinessRuleException("El examen visual indicado no existe.");
        if (await _db.Recetas.AnyAsync(x => x.ConsultaId == r.ConsultaId, ct))
            throw new BusinessRuleException("El examen visual ya tiene una receta.");

        var receta = new Receta
        {
            ConsultaId = r.ConsultaId,
            OdEsfera = r.OdEsfera, OdCilindro = r.OdCilindro, OdEje = r.OdEje, OdAdicion = r.OdAdicion,
            OiEsfera = r.OiEsfera, OiCilindro = r.OiCilindro, OiEje = r.OiEje, OiAdicion = r.OiAdicion,
            DistanciaPupilar = r.DistanciaPupilar, Observacion = r.Observacion,
            FechaEmision = DateTime.UtcNow
        };
        _db.Recetas.Add(receta);
        await _db.SaveChangesAsync(ct);
        return Map(receta);
    }

    public async Task<RecetaDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.Recetas.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct)
            ?? throw new NotFoundException("Receta no encontrada."));

    internal static RecetaDto Map(Receta r) => new()
    {
        Id = r.Id, ConsultaId = r.ConsultaId, FechaEmision = r.FechaEmision,
        OdEsfera = r.OdEsfera, OdCilindro = r.OdCilindro, OdEje = r.OdEje, OdAdicion = r.OdAdicion,
        OiEsfera = r.OiEsfera, OiCilindro = r.OiCilindro, OiEje = r.OiEje, OiAdicion = r.OiAdicion,
        DistanciaPupilar = r.DistanciaPupilar, Observacion = r.Observacion
    };
}
