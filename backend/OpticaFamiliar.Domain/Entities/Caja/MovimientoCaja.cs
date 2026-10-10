using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Caja
{
    [Table("movimiento_caja")]
    public class MovimientoCaja
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("caja_id")]
        [Required]
        public long CajaId { get; set; }

        [Column("usuario_id")]
        [Required]
        public long UsuarioId { get; set; }

        [Column("tipo_movimiento")]
        [Required]
        [MaxLength(20)]
        public string TipoMovimiento { get; set; } = string.Empty;

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("concepto")]
        [MaxLength(500)]
        public string? Concepto { get; set; }

        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento { get; set; }

        // Navigation properties
        public virtual Caja Caja { get; set; } = null!;
        public virtual Personas.Usuario Usuario { get; set; } = null!;
    }
}
