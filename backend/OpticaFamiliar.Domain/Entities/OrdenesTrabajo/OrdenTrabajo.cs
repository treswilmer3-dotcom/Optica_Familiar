using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Ventas;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;

namespace OpticaFamiliar.Domain.Entities.OrdenesTrabajo
{
    [Table("orden_trabajo")]
    public class OrdenTrabajo : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("venta_id")]
        public long? VentaId { get; set; }

        [Column("receta_id")]
        public long? RecetaId { get; set; }

        [Column("numero_orden")]
        [Required]
        [MaxLength(50)]
        public string NumeroOrden { get; set; } = string.Empty;

        [Column("fecha_ingreso")]
        [Required]
        public DateTime FechaIngreso { get; set; }

        [Column("fecha_entrega_estimada")]
        public DateTime? FechaEntregaEstimada { get; set; }

        [Column("fecha_entrega_real")]
        public DateTime? FechaEntregaReal { get; set; }

        [Column("estado")]
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = string.Empty;

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        // Navigation properties
        public virtual Ventas.Venta? Venta { get; set; }
        public virtual HistoriaClinica.Receta? Receta { get; set; }
    }
}
