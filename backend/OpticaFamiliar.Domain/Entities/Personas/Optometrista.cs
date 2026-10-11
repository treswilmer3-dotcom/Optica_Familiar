using OpticaFamiliar.Domain.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgendaMedica = OpticaFamiliar.Domain.Entities.AgendaMedica;
using HistoriaClinica = OpticaFamiliar.Domain.Entities.HistoriaClinica;

namespace OpticaFamiliar.Domain.Entities.Personas
{
    [Table("optometrista")]
    public class Optometrista : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("persona_id")]
        [Required]
        public long PersonaId { get; set; }

        [Column("usuario_id")]
        public long? UsuarioId { get; set; }

        [Column("numero_registro")]
        [MaxLength(50)]
        public string? NumeroRegistro { get; set; }

        [Column("especialidad")]
        [MaxLength(100)]
        public string? Especialidad { get; set; }

        [Column("firma")]
        [MaxLength(500)]
        public string? Firma { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Persona Persona { get; set; } = null!;
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<AgendaMedica.Cita> Citas { get; set; } = new List<AgendaMedica.Cita>();
        public virtual ICollection<HistoriaClinica.Consulta> Consultas { get; set; } = new List<HistoriaClinica.Consulta>();
    }
}
