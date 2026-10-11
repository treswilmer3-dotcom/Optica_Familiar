using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Configuracion
{
    [Table("parametro_catalogo")]
    public class ParametroCatalogo : IEmpresaOwned
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

        [Column("categoria")]
        [MaxLength(50)]
        public string? Categoria { get; set; }

        [Column("descripcion")]
        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Column("orden")]
        public int Orden { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
