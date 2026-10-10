using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.AgendaMedica;
using Inventario = OpticaFamiliar.Domain.Entities.Inventario;
using OpticaFamiliar.Domain.Entities.Compras;
using Ventas = OpticaFamiliar.Domain.Entities.Ventas;
using Caja = OpticaFamiliar.Domain.Entities.Caja;
using OpticaFamiliar.Domain.Entities.Configuracion;

namespace OpticaFamiliar.Domain.Entities.Organizacion
{
    [Table("sucursal")]
    public class Sucursal : BaseEntity
    {
        [Column("empresa_id")]
        [Required]
        public long EmpresaId { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Column("nombre")]
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column("direccion")]
        [MaxLength(500)]
        public string? Direccion { get; set; }

        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Column("correo")]
        [MaxLength(100)]
        public string? Correo { get; set; }

        [Column("ciudad")]
        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [Column("provincia")]
        [MaxLength(100)]
        public string? Provincia { get; set; }

        // Navigation properties
        public virtual Empresa Empresa { get; set; } = null!;
        public virtual SucursalConfiguracion? SucursalConfiguracion { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<AgendaMedica.Cita> Citas { get; set; } = new List<AgendaMedica.Cita>();
        public virtual ICollection<Inventario.Inventario> Inventarios { get; set; } = new List<Inventario.Inventario>();
        public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public virtual ICollection<Ventas.Venta> Ventas { get; set; } = new List<Ventas.Venta>();
        public virtual ICollection<Caja.Caja> Cajas { get; set; } = new List<Caja.Caja>();
        public virtual ICollection<NumeracionDocumento> NumeracionDocumentos { get; set; } = new List<NumeracionDocumento>();
    }
}
