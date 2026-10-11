using OpticaFamiliar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Organizacion
{
    [Table("empresa_configuracion")]
    public class EmpresaConfiguracion : IEmpresaOwned
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("empresa_id")]
        [Required]
        public long EmpresaId { get; set; }

        /// <summary>Logo como ruta estática (/brand/...) o data URL (png, jpeg o webp).</summary>
        [Column("logo")]
        public string? Logo { get; set; }

        [Column("color_primario")]
        [MaxLength(20)]
        public string? ColorPrimario { get; set; }

        [Column("color_secundario")]
        [MaxLength(20)]
        public string? ColorSecundario { get; set; }

        [Column("correo_notificaciones")]
        [MaxLength(100)]
        public string? CorreoNotificaciones { get; set; }

        [Column("telefono_contacto")]
        [MaxLength(20)]
        public string? TelefonoContacto { get; set; }

        [Column("direccion_matriz")]
        [MaxLength(500)]
        public string? DireccionMatriz { get; set; }

        [Column("sitio_web")]
        [MaxLength(200)]
        public string? SitioWeb { get; set; }

        [Column("mensaje_factura")]
        [MaxLength(500)]
        public string? MensajeFactura { get; set; }

        // Navigation properties
        public virtual Empresa Empresa { get; set; } = null!;
    }
}
