using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Personas;

namespace OpticaFamiliar.Domain.Entities.HistoriaClinica
{
    [Table("historia_clinica")]
    public class HistoriaClinica : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("paciente_id")]
        [Required]
        public long PacienteId { get; set; }

        [Column("numero_historia")]
        [Required]
        [MaxLength(50)]
        public string NumeroHistoria { get; set; } = string.Empty;

        [Column("fecha_apertura")]
        [Required]
        public DateTime FechaApertura { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Paciente Paciente { get; set; } = null!;
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
