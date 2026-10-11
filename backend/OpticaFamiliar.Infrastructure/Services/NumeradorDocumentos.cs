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

    public async Task<string> SiguienteAsync(long sucursalId, string tipoDocumento, CancellationToken ct)
    {
        var fila = await _db.NumeracionDocumentos
            .FromSqlInterpolated($"SELECT * FROM numeracion_documento WHERE sucursal_id = {sucursalId} AND tipo_documento = {tipoDocumento} FOR UPDATE")
            .FirstOrDefaultAsync(ct);

        if (fila == null)
        {
            var codigo = await _db.Sucursales.Where(x => x.Id == sucursalId).Select(x => x.Codigo).FirstOrDefaultAsync(ct)
                ?? throw new BusinessRuleException("La sucursal no existe.");
            fila = NumeracionDefecto.Crear(sucursalId, codigo, tipoDocumento);
            _db.NumeracionDocumentos.Add(fila);
        }

        if (fila.NumeroActual >= fila.NumeroFinal)
            throw new BusinessRuleException($"Se agotó la numeración de {tipoDocumento} para la sucursal.");

        fila.NumeroActual++;
        await _db.SaveChangesAsync(ct);
        return $"{fila.Serie}-{fila.NumeroActual:D9}";
    }
}

/// <summary>Numeración inicial por sucursal. La serie incluye el código de sucursal para que sea única en la empresa.</summary>
internal static class NumeracionDefecto
{
    public static readonly string[] Tipos = [TiposDocumento.Venta, TiposDocumento.OrdenTrabajo, TiposDocumento.HistoriaClinica];

    public static NumeracionDocumento Crear(long sucursalId, string codigoSucursal, string tipo) => new()
    {
        SucursalId = sucursalId,
        TipoDocumento = tipo,
        Serie = tipo switch
        {
            TiposDocumento.Venta => "001",
            TiposDocumento.OrdenTrabajo => $"OT-{codigoSucursal}",
            _ => $"HC-{codigoSucursal}"
        },
        NumeroActual = 0,
        NumeroFinal = 999_999_999,
        Estado = Estados.Activo
    };
}
