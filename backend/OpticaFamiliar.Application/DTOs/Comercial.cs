using System.ComponentModel.DataAnnotations;

namespace OpticaFamiliar.Application.DTOs;

public class ProductoRequest
{
    [Range(1, long.MaxValue)] public long CategoriaId { get; set; }
    public long? MarcaId { get; set; }
    [Required, MaxLength(50)] public string Codigo { get; set; } = string.Empty;
    [MaxLength(50)] public string? CodigoBarras { get; set; }
    [Required, MaxLength(200)] public string Nombre { get; set; } = string.Empty;
    [MaxLength(1000)] public string? Descripcion { get; set; }
    [Range(0, 1_000_000)] public decimal Costo { get; set; }
    [Range(0, 1_000_000)] public decimal Precio { get; set; }
    public bool RequiereFormula { get; set; }
}

public class ProductoDto : ProductoRequest
{
    public long Id { get; set; }
    public string? Categoria { get; set; }
    public string? Estado { get; set; }
}

public class OrdenTrabajoRequest
{
    [Range(1, long.MaxValue)] public long RecetaId { get; set; }
    public DateTime? FechaEntregaEstimada { get; set; }
    [MaxLength(1000)] public string? Observaciones { get; set; }
}

public class CambiarEstadoOrdenRequest
{
    [Required] public string Estado { get; set; } = string.Empty;
}

public class OrdenTrabajoDto
{
    public long Id { get; set; }
    public long? VentaId { get; set; }
    public long? RecetaId { get; set; }
    public string NumeroOrden { get; set; } = string.Empty;
    public DateTime FechaIngreso { get; set; }
    public DateTime? FechaEntregaEstimada { get; set; }
    public DateTime? FechaEntregaReal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}

public class VentaDetalleRequest
{
    [Range(1, long.MaxValue)] public long ProductoId { get; set; }
    [Range(1, 10_000)] public int Cantidad { get; set; }
    /// <summary>Si se omite, se usa el precio vigente del producto.</summary>
    [Range(0, 1_000_000)] public decimal? PrecioUnitario { get; set; }
    [Range(0, 1_000_000)] public decimal Descuento { get; set; }
}

public class PagoRequest
{
    [Required, MaxLength(20)] public string MetodoPago { get; set; } = string.Empty;
    [Range(0.01, 10_000_000)] public decimal Valor { get; set; }
    [MaxLength(100)] public string? Referencia { get; set; }
}

public class VentaRequest
{
    [Range(1, long.MaxValue)] public long ClienteId { get; set; }
    /// <summary>Orden de trabajo (sin venta) que se vincula a esta venta.</summary>
    public long? OrdenTrabajoId { get; set; }
    [Required, MinLength(1)] public List<VentaDetalleRequest> Detalles { get; set; } = [];
    public List<PagoRequest> Pagos { get; set; } = [];
}

public class VentaDetalleDto
{
    public long Id { get; set; }
    public long ProductoId { get; set; }
    public string? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal Subtotal { get; set; }
}

public class PagoDto
{
    public long Id { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string? Referencia { get; set; }
    public DateTime FechaPago { get; set; }
}

public class VentaDto
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public long UsuarioId { get; set; }
    public long SucursalId { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaVenta { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Iva { get; set; }
    public decimal Total { get; set; }
    public decimal TotalPagado { get; set; }
    public string? Estado { get; set; }
    public List<VentaDetalleDto> Detalles { get; set; } = [];
    public List<PagoDto> Pagos { get; set; } = [];
}
