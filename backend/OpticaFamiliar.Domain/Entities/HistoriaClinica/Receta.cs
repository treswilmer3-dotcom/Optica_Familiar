using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.OrdenesTrabajo;

namespace OpticaFamiliar.Domain.Entities.HistoriaClinica
{
    [Table("receta")]
    public class Receta
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("consulta_id")]
        [Required]
        public long ConsultaId { get; set; }

        [Column("od_esfera")]
        public decimal? OdEsfera { get; set; }

        [Column("od_cilindro")]
        public decimal? OdCilindro { get; set; }

        [Column("od_eje")]
        public int? OdEje { get; set; }

        [Column("od_adicion")]
        public decimal? OdAdicion { get; set; }

        [Column("oi_esfera")]
        public decimal? OiEsfera { get; set; }

        [Column("oi_cilindro")]
        public decimal? OiCilindro { get; set; }

        [Column("oi_eje")]
        public int? OiEje { get; set; }

        [Column("oi_adicion")]
        public decimal? OiAdicion { get; set; }

        [Column("distancia_pupilar")]
        public decimal? DistanciaPupilar { get; set; }

        [Column("observacion")]
        [MaxLength(1000)]
        public string? Observacion { get; set; }

        [Column("fecha_emision")]
        public DateTime FechaEmision { get; set; }

        // Navigation properties
        public virtual Consulta Consulta { get; set; } = null!;
        public virtual ICollection<OrdenesTrabajo.OrdenTrabajo> OrdenesTrabajo { get; set; } = new List<OrdenesTrabajo.OrdenTrabajo>();
    }
}
