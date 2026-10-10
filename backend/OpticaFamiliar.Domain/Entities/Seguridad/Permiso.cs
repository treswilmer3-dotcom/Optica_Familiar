using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Seguridad
{
    [Table("permiso")]
    public class Permiso
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Column("nombre")]
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Column("modulo")]
        [MaxLength(50)]
        public string? Modulo { get; set; }

        // Navigation properties
        public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
    }
}
