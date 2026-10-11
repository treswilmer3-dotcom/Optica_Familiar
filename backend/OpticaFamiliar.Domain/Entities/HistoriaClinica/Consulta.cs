using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.AgendaMedica;

namespace OpticaFamiliar.Domain.Entities.HistoriaClinica
{
    [Table("consulta")]
    public class Consulta : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("historia_clinica_id")]
        [Required]
        public long HistoriaClinicaId { get; set; }

        [Column("optometrista_id")]
        [Required]
        public long OptometristaId { get; set; }

        [Column("cita_id")]
        public long? CitaId { get; set; }

        [Column("fecha_consulta")]
        [Required]
        public DateTime FechaConsulta { get; set; }

        [Column("motivo_consulta")]
        [MaxLength(500)]
        public string? MotivoConsulta { get; set; }

        [Column("diagnostico")]
        [MaxLength(1000)]
        public string? Diagnostico { get; set; }

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        [Column("recomendaciones")]
        [MaxLength(1000)]
        public string? Recomendaciones { get; set; }

        // Navigation properties
        public virtual HistoriaClinica HistoriaClinica { get; set; } = null!;
        public virtual Optometrista Optometrista { get; set; } = null!;
        public virtual Cita? Cita { get; set; }
        public virtual Receta? Receta { get; set; }
    }
}
