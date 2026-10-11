using OpticaFamiliar.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpticaFamiliar.Domain.Entities.Auditoria
{
    [Table("auditoria")]
    public class Auditoria : IEmpresaOwned
    {
        [Column("empresa_id")]
        public long EmpresaId { get; set; }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("usuario_id")]
        public long? UsuarioId { get; set; }

        [Column("tabla")]
        [Required]
        [MaxLength(100)]
        public string Tabla { get; set; } = string.Empty;

        [Column("registro_id")]
        public long? RegistroId { get; set; }

        [Column("accion")]
        [Required]
        [MaxLength(20)]
        public string Accion { get; set; } = string.Empty;

        [Column("valor_anterior")]
        public string? ValorAnterior { get; set; }

        [Column("valor_nuevo")]
        public string? ValorNuevo { get; set; }

        [Column("ip")]
        [MaxLength(50)]
        public string? Ip { get; set; }

        [Column("fecha_evento")]
        public DateTime FechaEvento { get; set; }

        // Navigation properties
        public virtual Personas.Usuario? Usuario { get; set; }
    }
}
