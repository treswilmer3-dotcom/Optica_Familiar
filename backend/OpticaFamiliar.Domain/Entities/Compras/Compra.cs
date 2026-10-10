using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Compras
{
    [Table("compra")]
    public class Compra
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("proveedor_id")]
        [Required]
        public long ProveedorId { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("numero_documento")]
        [Required]
        [MaxLength(50)]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Column("fecha_compra")]
        [Required]
        public DateTime FechaCompra { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("iva")]
        public decimal Iva { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("estado")]
        [MaxLength(20)]
        public string? Estado { get; set; }

        // Navigation properties
        public virtual Proveedores.Proveedor Proveedor { get; set; } = null!;
        public virtual Organizacion.Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
    }
}
