using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Domain.Entities.Configuracion;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

/// <summary>
/// Genera números correlativos por sucursal y tipo de documento.
/// Debe invocarse dentro de una transacción: bloquea la fila (FOR UPDATE) para evitar duplicados concurrentes.
/// </summary>
internal sealed class NumeradorDocumentos
{
    private readonly AppDbContext _db;

    public NumeradorDocumentos(AppDbContext db) => _db = db;

    public async Task<string> SiguienteAsync(long sucursalId, string tipoDocumento, string serieDefecto, CancellationToken ct)
    {
        var fila = await _db.NumeracionDocumentos
            .FromSqlInterpolated($"SELECT * FROM numeracion_documento WHERE sucursal_id = {sucursalId} AND tipo_documento = {tipoDocumento} FOR UPDATE")
            .FirstOrDefaultAsync(ct);

        if (fila == null)
        {
            fila = new NumeracionDocumento
            {
                SucursalId = sucursalId,
                TipoDocumento = tipoDocumento,
                Serie = serieDefecto,
                NumeroActual = 0,
                NumeroFinal = 999_999_999,
                Estado = Estados.Activo
            };
            _db.NumeracionDocumentos.Add(fila);
        }

        if (fila.NumeroActual >= fila.NumeroFinal)
            throw new BusinessRuleException($"Se agotó la numeración de {tipoDocumento} para la sucursal.");

        fila.NumeroActual++;
        await _db.SaveChangesAsync(ct);
        return $"{fila.Serie}-{fila.NumeroActual:D9}";
    }
}
