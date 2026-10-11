using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Ventas;

namespace OpticaFamiliar.Domain.Entities.Personas
{
    [Table("cliente")]
    public class Cliente : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("persona_id")]
        [Required]
        public long PersonaId { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        // Navigation properties
        public virtual Persona Persona { get; set; } = null!;
        public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
