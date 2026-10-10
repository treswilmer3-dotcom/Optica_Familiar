using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;
using Inventario = OpticaFamiliar.Domain.Entities.Inventario;
using OpticaFamiliar.Domain.Entities.Compras;
using Ventas = OpticaFamiliar.Domain.Entities.Ventas;
using OpticaFamiliar.Domain.Entities.Transferencias;

namespace OpticaFamiliar.Domain.Entities.Productos
{
    [Table("producto")]
    public class Producto : BaseEntity
    {
        [Column("categoria_id")]
        [Required]
        public long CategoriaId { get; set; }

        [Column("marca_id")]
        public long? MarcaId { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Column("codigo_barras")]
        [MaxLength(50)]
        public string? CodigoBarras { get; set; }

        [Column("nombre")]
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Column("descripcion")]
        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        [Column("costo")]
        public decimal Costo { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("stock_minimo")]
        public int StockMinimo { get; set; }

        [Column("stock_maximo")]
        public int StockMaximo { get; set; }

        [Column("requiere_formula")]
        public bool RequiereFormula { get; set; }

        // Navigation properties
        public virtual CategoriaProducto Categoria { get; set; } = null!;
        public virtual Marca? Marca { get; set; }
        public virtual ICollection<Inventario.Inventario> Inventarios { get; set; } = new List<Inventario.Inventario>();
        public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();
        public virtual ICollection<Ventas.VentaDetalle> VentaDetalles { get; set; } = new List<Ventas.VentaDetalle>();
        public virtual ICollection<TransferenciaDetalle> TransferenciaDetalles { get; set; } = new List<TransferenciaDetalle>();
    }
}
