using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Ventas
{
    [Table("venta_detalle")]
    public class VentaDetalle
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("venta_id")]
        [Required]
        public long VentaId { get; set; }

        [Column("producto_id")]
        [Required]
        public long ProductoId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        // Navigation properties
        public virtual Venta Venta { get; set; } = null!;
        public virtual Productos.Producto Producto { get; set; } = null!;
    }
}
