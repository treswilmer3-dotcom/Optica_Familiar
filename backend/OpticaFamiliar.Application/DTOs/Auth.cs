using System.ComponentModel.DataAnnotations;

namespace OpticaFamiliar.Application.DTOs;

public class LoginRequest
{
    [Required, MaxLength(50)] public string CodigoEmpresa { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string Username { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEn { get; set; }
    public long UsuarioId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public long SucursalId { get; set; }
    public long EmpresaId { get; set; }
    public string? Empresa { get; set; }
}
