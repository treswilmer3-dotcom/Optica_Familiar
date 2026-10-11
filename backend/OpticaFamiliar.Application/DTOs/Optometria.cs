using System.ComponentModel.DataAnnotations;

namespace OpticaFamiliar.Application.DTOs;

public class ExamenVisualRequest
{
    [Range(1, long.MaxValue)] public long ClienteId { get; set; }
    /// <summary>Si se omite, se usa el optometrista asociado al usuario autenticado.</summary>
    public long? OptometristaId { get; set; }
    public long? CitaId { get; set; }
    [MaxLength(500)] public string? MotivoConsulta { get; set; }
    [MaxLength(1000)] public string? Diagnostico { get; set; }
    [MaxLength(1000)] public string? Observaciones { get; set; }
    [MaxLength(1000)] public string? Recomendaciones { get; set; }
}

public class ExamenVisualDto
{
    public long Id { get; set; }
    public long ClienteId { get; set; }
    public string? ClienteNombre { get; set; }
    public long HistoriaClinicaId { get; set; }
    public string? NumeroHistoria { get; set; }
    public long OptometristaId { get; set; }
    public long? CitaId { get; set; }
    public DateTime FechaConsulta { get; set; }
    public string? MotivoConsulta { get; set; }
    public string? Diagnostico { get; set; }
    public string? Observaciones { get; set; }
    public string? Recomendaciones { get; set; }
    public RecetaDto? Receta { get; set; }
}

public class RecetaRequest
{
    [Range(1, long.MaxValue)] public long ConsultaId { get; set; }
    [Range(-30, 30)] public decimal? OdEsfera { get; set; }
    [Range(-10, 10)] public decimal? OdCilindro { get; set; }
    [Range(0, 180)] public int? OdEje { get; set; }
    [Range(0, 5)] public decimal? OdAdicion { get; set; }
    [Range(-30, 30)] public decimal? OiEsfera { get; set; }
    [Range(-10, 10)] public decimal? OiCilindro { get; set; }
    [Range(0, 180)] public int? OiEje { get; set; }
    [Range(0, 5)] public decimal? OiAdicion { get; set; }
    [Range(20, 100)] public decimal? DistanciaPupilar { get; set; }
    [MaxLength(1000)] public string? Observacion { get; set; }
}

public class RecetaDto : RecetaRequest
{
    public long Id { get; set; }
    public DateTime FechaEmision { get; set; }
}
