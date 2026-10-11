using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Productos;

namespace OpticaFamiliar.Domain.Entities.Inventario
{
    [Table("inventario")]
    public class Inventario : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("producto_id")]
        [Required]
        public long ProductoId { get; set; }

        [Column("stock_actual")]
        public int StockActual { get; set; }

        [Column("stock_reservado")]
        public int StockReservado { get; set; }

        [Column("ultima_actualizacion")]
        public DateTime UltimaActualizacion { get; set; }

        // Navigation properties
        public virtual Sucursal Sucursal { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
        public virtual ICollection<MovimientoInventario> MovimientosInventario { get; set; } = new List<MovimientoInventario>();
    }
}
