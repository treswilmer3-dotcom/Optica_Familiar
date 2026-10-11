using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Domain.Entities.Seguridad;
using Inventario = OpticaFamiliar.Domain.Entities.Inventario;
using Caja = OpticaFamiliar.Domain.Entities.Caja;
using OpticaFamiliar.Domain.Entities.Transferencias;
using Auditoria = OpticaFamiliar.Domain.Entities.Auditoria;
using Ventas = OpticaFamiliar.Domain.Entities.Ventas;

namespace OpticaFamiliar.Domain.Entities.Personas
{
    [Table("usuario")]
    public class Usuario : BaseEntity, IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Column("persona_id")]
        public long? PersonaId { get; set; }

        [Column("rol_id")]
        [Required]
        public long RolId { get; set; }

        [Column("sucursal_id")]
        [Required]
        public long SucursalId { get; set; }

        [Column("username")]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("ultimo_acceso")]
        public DateTime? UltimoAcceso { get; set; }

        // Navigation properties
        public virtual Persona? Persona { get; set; }
        public virtual Seguridad.Rol Rol { get; set; } = null!;
        public virtual Organizacion.Sucursal Sucursal { get; set; } = null!;
        public virtual ICollection<Inventario.MovimientoInventario> MovimientosInventario { get; set; } = new List<Inventario.MovimientoInventario>();
        public virtual ICollection<Ventas.Venta> Ventas { get; set; } = new List<Ventas.Venta>();
        public virtual ICollection<Caja.MovimientoCaja> MovimientosCaja { get; set; } = new List<Caja.MovimientoCaja>();
        public virtual ICollection<Transferencias.Transferencia> TransferenciasSolicitadas { get; set; } = new List<Transferencias.Transferencia>();
        public virtual ICollection<Transferencias.Transferencia> TransferenciasAprobadas { get; set; } = new List<Transferencias.Transferencia>();
        public virtual ICollection<Auditoria.Auditoria> Auditorias { get; set; } = new List<Auditoria.Auditoria>();
    }
}
