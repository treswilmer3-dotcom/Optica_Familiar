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

        /// <summary>Identificación fiscal del país (RUC en Ecuador).</summary>
        [Column("identificacion_fiscal")]
        [MaxLength(30)]
        public string IdentificacionFiscal { get; set; } = string.Empty;

        /// <summary>País ISO 3166-1 alfa-2 (EC, CO, PE...).</summary>
        [Column("pais")]
        [MaxLength(2)]
        public string Pais { get; set; } = "EC";

        /// <summary>Moneda ISO 4217 (USD, COP...).</summary>
        [Column("moneda")]
        [MaxLength(3)]
        public string Moneda { get; set; } = "USD";

        /// <summary>Zona horaria IANA (America/Guayaquil...).</summary>
        [Column("zona_horaria")]
        [MaxLength(50)]
        public string ZonaHoraria { get; set; } = "America/Guayaquil";

        [Column("idioma")]
        [MaxLength(5)]
        public string Idioma { get; set; } = "es";

        /// <summary>Porcentaje de IVA/impuesto por defecto de la empresa.</summary>
        [Column("iva_porcentaje")]
        public decimal IvaPorcentaje { get; set; } = 15m;

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
