using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Compras
{
    [Table("compra_detalle")]
    public class CompraDetalle : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("compra_id")]
        [Required]
        public long CompraId { get; set; }

        [Column("producto_id")]
        [Required]
        public long ProductoId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("costo_unitario")]
        public decimal CostoUnitario { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        // Navigation properties
        public virtual Compra Compra { get; set; } = null!;
        public virtual Productos.Producto Producto { get; set; } = null!;
    }
}
