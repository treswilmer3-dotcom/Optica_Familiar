using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;

namespace OpticaFamiliar.Domain.Entities.Organizacion
{
    [Table("empresa")]
    public class Empresa : BaseEntity
    {
        [Column("codigo")]
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Column("razon_social")]
        [Required]
        [MaxLength(200)]
        public string RazonSocial { get; set; } = string.Empty;

        [Column("nombre_comercial")]
        [Required]
        [MaxLength(200)]
        public string NombreComercial { get; set; } = string.Empty;

        [Column("ruc")]
        [Required]
        [MaxLength(20)]
        public string Ruc { get; set; } = string.Empty;

        [Column("direccion")]
        [MaxLength(500)]
        public string? Direccion { get; set; }

        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Column("correo")]
        [MaxLength(100)]
        public string? Correo { get; set; }

        [Column("sitio_web")]
        [MaxLength(200)]
        public string? SitioWeb { get; set; }

        // Navigation properties
        public virtual EmpresaConfiguracion? EmpresaConfiguracion { get; set; }
        public virtual ICollection<Sucursal> Sucursales { get; set; } = new List<Sucursal>();
    }
}
