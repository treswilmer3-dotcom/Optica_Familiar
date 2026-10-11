using System.ComponentModel.DataAnnotations;

namespace OpticaFamiliar.Application.DTOs;

public class ClienteRequest
{
    [MaxLength(20)] public string? TipoIdentificacion { get; set; }
    [Required, MaxLength(20)] public string NumeroIdentificacion { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Nombres { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Apellidos { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    [MaxLength(10)] public string? Genero { get; set; }
    [MaxLength(20)] public string? Telefono { get; set; }
    [MaxLength(20)] public string? Celular { get; set; }
    [EmailAddress, MaxLength(100)] public string? Correo { get; set; }
    [MaxLength(500)] public string? Direccion { get; set; }
    [MaxLength(1000)] public string? Observaciones { get; set; }
}

public class ClienteDto : ClienteRequest
{
    public long Id { get; set; }
    public long PersonaId { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class HistorialClienteDto
{
    public ClienteDto Cliente { get; set; } = null!;
    public List<ExamenVisualDto> Examenes { get; set; } = [];
    public List<OrdenTrabajoDto> Ordenes { get; set; } = [];
    public List<VentaDto> Ventas { get; set; } = [];
}
