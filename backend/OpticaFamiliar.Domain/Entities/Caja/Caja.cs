using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Caja
{
    [Table("caja")]
    public class Caja
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Column("nombre")]
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Organizacion.Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<MovimientoCaja> MovimientosCaja { get; set; } = new List<MovimientoCaja>();
    }
}
