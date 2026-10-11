using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Domain.Entities.Productos;

namespace OpticaFamiliar.Infrastructure.Seed;

/// <summary>Datos base que toda empresa nueva necesita para operar.</summary>
internal static class Aprovisionamiento
{
    public static IEnumerable<CategoriaProducto> CategoriasBase(long empresaId) =>
    [
        new() { EmpresaId = empresaId, Codigo = "MONTURA", Nombre = "Monturas", Estado = Estados.Activo },
        new() { EmpresaId = empresaId, Codigo = "LENTE", Nombre = "Lentes", Estado = Estados.Activo },
        new() { EmpresaId = empresaId, Codigo = "SERVICIO", Nombre = "Servicios", Estado = Estados.Activo }
    ];
}
