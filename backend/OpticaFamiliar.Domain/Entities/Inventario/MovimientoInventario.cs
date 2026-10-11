using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Personas;

namespace OpticaFamiliar.Domain.Entities.Inventario
{
    [Table("movimiento_inventario")]
    public class MovimientoInventario : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("inventario_id")]
        [Required]
        public long InventarioId { get; set; }

        [Column("usuario_id")]
        [Required]
        public long UsuarioId { get; set; }

        [Column("tipo_movimiento")]
        [Required]
        [MaxLength(20)]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("stock_anterior")]
        public int StockAnterior { get; set; }

        [Column("stock_nuevo")]
        public int StockNuevo { get; set; }

        [Column("descripcion")]
        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento { get; set; }

        // Navigation properties
        public virtual Inventario Inventario { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
