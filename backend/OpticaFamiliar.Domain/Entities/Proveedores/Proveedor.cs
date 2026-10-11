using OpticaFamiliar.Domain.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Proveedores
{
    [Table("proveedor")]
    public class Proveedor : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("ruc")]
        [Required]
        [MaxLength(20)]
        public string Ruc { get; set; } = string.Empty;

        [Column("razon_social")]
        [Required]
        [MaxLength(200)]
        public string RazonSocial { get; set; } = string.Empty;

        [Column("nombre_comercial")]
        [MaxLength(200)]
        public string? NombreComercial { get; set; }

        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Column("correo")]
        [MaxLength(100)]
        public string? Correo { get; set; }

        [Column("direccion")]
        [MaxLength(500)]
        public string? Direccion { get; set; }

        [Column("contacto")]
        [MaxLength(100)]
        public string? Contacto { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual ICollection<Compras.Compra> Compras { get; set; } = new List<Compras.Compra>();
    }
}
