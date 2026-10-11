using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Transferencias
{
    [Table("transferencia_detalle")]
    public class TransferenciaDetalle : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("transferencia_id")]
        [Required]
        public long TransferenciaId { get; set; }

        [Column("producto_id")]
        [Required]
        public long ProductoId { get; set; }

        [Column("cantidad_solicitada")]
        public int CantidadSolicitada { get; set; }

        [Column("cantidad_aprobada")]
        public int? CantidadAprobada { get; set; }

        [Column("cantidad_enviada")]
        public int? CantidadEnviada { get; set; }

        [Column("cantidad_recibida")]
        public int? CantidadRecibida { get; set; }

        [Column("observacion")]
        [MaxLength(500)]
        public string? Observacion { get; set; }

        // Navigation properties
        public virtual Transferencia Transferencia { get; set; } = null!;
        public virtual Productos.Producto Producto { get; set; } = null!;
    }
}
