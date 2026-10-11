using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Configuracion
{
    [Table("configuracion_sistema")]
    public class ConfiguracionSistema : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

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

        [Column("valor")]
        [MaxLength(500)]
        public string? Valor { get; set; }

        [Column("tipo_dato")]
        [MaxLength(20)]
        public string? TipoDato { get; set; }

        [Column("categoria")]
        [MaxLength(50)]
        public string? Categoria { get; set; }

        [Column("editable")]
        public bool Editable { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }
    }
}
