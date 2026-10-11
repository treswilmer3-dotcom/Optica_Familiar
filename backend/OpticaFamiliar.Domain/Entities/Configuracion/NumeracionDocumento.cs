using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Configuracion
{
    [Table("numeracion_documento")]
    public class NumeracionDocumento : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("tipo_documento")]
        [Required]
        [MaxLength(50)]
        public string TipoDocumento { get; set; } = string.Empty;

        [Column("serie")]
        [Required]
        [MaxLength(20)]
        public string Serie { get; set; } = string.Empty;

        [Column("numero_actual")]
        public int NumeroActual { get; set; }

        [Column("numero_final")]
        public int NumeroFinal { get; set; }

        [Column("reinicio_anual")]
        public bool ReinicioAnual { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Organizacion.Sucursal Sucursal { get; set; } = null!;
    }
}
