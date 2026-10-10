using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Seguridad
{
    [Table("rol_permiso")]
    public class RolPermiso
    {
        [Column("rol_id")]
        public long RolId { get; set; }

        [Column("permiso_id")]
        public long PermisoId { get; set; }

        // Navigation properties
        public virtual Rol Rol { get; set; } = null!;
        public virtual Permiso Permiso { get; set; } = null!;
    }
}
