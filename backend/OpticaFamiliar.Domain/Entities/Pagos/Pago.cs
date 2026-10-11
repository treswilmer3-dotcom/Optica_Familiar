using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Pagos
{
    [Table("pago")]
    public class Pago : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("venta_id")]
        [Required]
        public long VentaId { get; set; }

        [Column("metodo_pago")]
        [Required]
        [MaxLength(20)]
        public string MetodoPago { get; set; } = string.Empty;

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("referencia")]
        [MaxLength(100)]
        public string? Referencia { get; set; }

        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; }

        // Navigation properties
        public virtual Ventas.Venta Venta { get; set; } = null!;
    }
}
