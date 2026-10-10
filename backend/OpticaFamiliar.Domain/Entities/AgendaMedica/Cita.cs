using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.HistoriaClinica;

namespace OpticaFamiliar.Domain.Entities.AgendaMedica
{
    [Table("cita")]
    public class Cita
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("paciente_id")]
        [Required]
        public long PacienteId { get; set; }

        [Column("optometrista_id")]
        [Required]
        public long OptometristaId { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("fecha")]
        [Required]
        public DateTime Fecha { get; set; }

        [Column("hora_inicio")]
        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Column("hora_fin")]
        [Required]
        public TimeSpan HoraFin { get; set; }

        [Column("motivo")]
        [MaxLength(500)]
        public string? Motivo { get; set; }

        [Column("estado")]
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = string.Empty;

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        // Navigation properties
        public virtual Paciente Paciente { get; set; } = null!;
        public virtual Optometrista Optometrista { get; set; } = null!;
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual Consulta? Consulta { get; set; }
    }
}
