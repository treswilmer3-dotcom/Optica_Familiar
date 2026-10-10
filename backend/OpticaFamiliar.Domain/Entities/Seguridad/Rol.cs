using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;
using OpticaFamiliar.Domain.Entities.Personas;

namespace OpticaFamiliar.Domain.Entities.Seguridad
{
    [Table("rol")]
    public class Rol : BaseEntity
    {
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

        // Navigation properties
        public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
