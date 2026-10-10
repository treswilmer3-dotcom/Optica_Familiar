using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Organizacion
{
    [Table("sucursal_configuracion")]
    public class SucursalConfiguracion
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("nombre_impresora")]
        [MaxLength(100)]
        public string? NombreImpresora { get; set; }

        [Column("impresora_facturas")]
        [MaxLength(100)]
        public string? ImpresoraFacturas { get; set; }

        [Column("impresora_etiquetas")]
        [MaxLength(100)]
        public string? ImpresoraEtiquetas { get; set; }

        [Column("correo_sucursal")]
        [MaxLength(100)]
        public string? CorreoSucursal { get; set; }

        [Column("telefono_sucursal")]
        [MaxLength(20)]
        public string? TelefonoSucursal { get; set; }

        [Column("direccion_sucursal")]
        [MaxLength(500)]
        public string? DireccionSucursal { get; set; }

        // Navigation properties
        public virtual Sucursal Sucursal { get; set; } = null!;
    }
}
