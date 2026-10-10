using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Personas;

namespace OpticaFamiliar.Domain.Entities.Transferencias
{
    [Table("transferencia")]
    public class Transferencia
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("numero_transferencia")]
        [Required]
        [MaxLength(50)]
        public string NumeroTransferencia { get; set; } = string.Empty;

        [Column("sucursal_origen_id")]
        [Required]
        public long SucursalOrigenId { get; set; }

        [Column("sucursal_destino_id")]
        [Required]
        public long SucursalDestinoId { get; set; }

        [Column("usuario_solicita_id")]
        [Required]
        public long UsuarioSolicitaId { get; set; }

        [Column("usuario_aprueba_id")]
        public long? UsuarioApruebaId { get; set; }

        [Column("fecha_solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [Column("fecha_aprobacion")]
        public DateTime? FechaAprobacion { get; set; }

        [Column("fecha_envio")]
        public DateTime? FechaEnvio { get; set; }

        [Column("fecha_recepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [Column("observaciones")]
        [MaxLength(1000)]
        public string? Observaciones { get; set; }

        [Column("estado")]
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = string.Empty;

        // Navigation properties
        public virtual Organizacion.Sucursal SucursalOrigen { get; set; } = null!;
        public virtual Organizacion.Sucursal SucursalDestino { get; set; } = null!;
        public virtual Personas.Usuario UsuarioSolicita { get; set; } = null!;
        public virtual Personas.Usuario? UsuarioAprueba { get; set; }
        public virtual ICollection<TransferenciaDetalle> TransferenciaDetalles { get; set; } = new List<TransferenciaDetalle>();
    }
}
