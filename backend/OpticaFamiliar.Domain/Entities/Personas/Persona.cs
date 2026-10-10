using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpticaFamiliar.Domain.Entities.Common;

namespace OpticaFamiliar.Domain.Entities.Personas
{
    [Table("persona")]
    public class Persona : BaseEntity
    {
        [Column("tipo_identificacion")]
        [MaxLength(20)]
        public string? TipoIdentificacion { get; set; }

        [Column("numero_identificacion")]
        [MaxLength(20)]
        public string? NumeroIdentificacion { get; set; }

        [Column("nombres")]
        [MaxLength(100)]
        public string? Nombres { get; set; }

        [Column("apellidos")]
        [MaxLength(100)]
        public string? Apellidos { get; set; }

        [Column("fecha_nacimiento")]
        public DateTime? FechaNacimiento { get; set; }

        [Column("genero")]
        [MaxLength(10)]
        public string? Genero { get; set; }

        [Column("telefono")]
        [MaxLength(20)]
        public string? Telefono { get; set; }

        [Column("celular")]
        [MaxLength(20)]
        public string? Celular { get; set; }

        [Column("correo")]
        [MaxLength(100)]
        public string? Correo { get; set; }

        [Column("direccion")]
        [MaxLength(500)]
        public string? Direccion { get; set; }

        // Navigation properties
        public virtual Cliente? Cliente { get; set; }
        public virtual Paciente? Paciente { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual Optometrista? Optometrista { get; set; }
    }
}
