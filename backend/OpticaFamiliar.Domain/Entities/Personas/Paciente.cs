using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgendaMedica = OpticaFamiliar.Domain.Entities.AgendaMedica;
using HistoriaClinica = OpticaFamiliar.Domain.Entities.HistoriaClinica;

namespace OpticaFamiliar.Domain.Entities.Personas
{
    [Table("paciente")]
    public class Paciente
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("persona_id")]
        [Required]
        public long PersonaId { get; set; }

        [Column("ocupacion")]
        [MaxLength(100)]
        public string? Ocupacion { get; set; }

        [Column("empresa")]
        [MaxLength(200)]
        public string? Empresa { get; set; }

        [Column("alergias")]
        [MaxLength(500)]
        public string? Alergias { get; set; }

        [Column("antecedentes_medicos")]
        [MaxLength(1000)]
        public string? AntecedentesMedicos { get; set; }

        [Column("observaciones_generales")]
        [MaxLength(1000)]
        public string? ObservacionesGenerales { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Persona Persona { get; set; } = null!;
        public virtual ICollection<AgendaMedica.Cita> Citas { get; set; } = new List<AgendaMedica.Cita>();
        public virtual ICollection<HistoriaClinica.HistoriaClinica> HistoriasClinicas { get; set; } = new List<HistoriaClinica.HistoriaClinica>();
    }
}
