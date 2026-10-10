using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Personas;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Pagos;
using OpticaFamiliar.Domain.Entities.OrdenesTrabajo;

namespace OpticaFamiliar.Domain.Entities.Ventas
{
    [Table("venta")]
    public class Venta
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("cliente_id")]
        [Required]
        public long ClienteId { get; set; }

        [Column("usuario_id")]
        [Required]
        public long UsuarioId { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("numero_factura")]
        [Required]
        [MaxLength(50)]
        public string NumeroFactura { get; set; } = string.Empty;

        [Column("fecha_venta")]
        [Required]
        public DateTime FechaVenta { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; }

        [Column("iva")]
        public decimal Iva { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Personas.Cliente Cliente { get; set; } = null!;
        public virtual Personas.Usuario Usuario { get; set; } = null!;
        public virtual Organizacion.Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
        public virtual ICollection<Pagos.Pago> Pagos { get; set; } = new List<Pagos.Pago>();
        public virtual OrdenesTrabajo.OrdenTrabajo? OrdenTrabajo { get; set; }
    }
}
