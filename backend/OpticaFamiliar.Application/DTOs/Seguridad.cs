using System.ComponentModel.DataAnnotations;

namespace OpticaFamiliar.Application.DTOs;

public class EmpresaRequest
{
    [Required, MaxLength(50)] public string Codigo { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string RazonSocial { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string NombreComercial { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string Ruc { get; set; } = string.Empty;
    [MaxLength(500)] public string? Direccion { get; set; }
    [MaxLength(20)] public string? Telefono { get; set; }
    [EmailAddress, MaxLength(100)] public string? Correo { get; set; }
    [MaxLength(200)] public string? SitioWeb { get; set; }
}

public class EmpresaDto : EmpresaRequest
{
    public long Id { get; set; }
    public string? Estado { get; set; }
}

public class SucursalRequest
{
    [Range(1, long.MaxValue)] public long EmpresaId { get; set; }
    [Required, MaxLength(50)] public string Codigo { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Nombre { get; set; } = string.Empty;
    [MaxLength(500)] public string? Direccion { get; set; }
    [MaxLength(20)] public string? Telefono { get; set; }
    [EmailAddress, MaxLength(100)] public string? Correo { get; set; }
    [MaxLength(100)] public string? Ciudad { get; set; }
    [MaxLength(100)] public string? Provincia { get; set; }
}

public class SucursalDto : SucursalRequest
{
    public long Id { get; set; }
    public string? Estado { get; set; }
}

public class RolDto
{
    public long Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class UsuarioCreateRequest
{
    [Required, MaxLength(50)] public string Username { get; set; } = string.Empty;
    [Required, MinLength(8), MaxLength(100)] public string Password { get; set; } = string.Empty;
    [Range(1, long.MaxValue)] public long RolId { get; set; }
    [Range(1, long.MaxValue)] public long SucursalId { get; set; }
    [Required, MaxLength(100)] public string Nombres { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string Apellidos { get; set; } = string.Empty;
    [MaxLength(20)] public string? TipoIdentificacion { get; set; }
    [MaxLength(20)] public string? NumeroIdentificacion { get; set; }
    [EmailAddress, MaxLength(100)] public string? Correo { get; set; }
    [MaxLength(20)] public string? Celular { get; set; }
}

public class UsuarioUpdateRequest
{
    [Range(1, long.MaxValue)] public long RolId { get; set; }
    [Range(1, long.MaxValue)] public long SucursalId { get; set; }
    [Required] public string Estado { get; set; } = "ACTIVO";
}

public class CambiarPasswordRequest
{
    public string? PasswordActual { get; set; }
    [Required, MinLength(8), MaxLength(100)] public string PasswordNueva { get; set; } = string.Empty;
}

public class UsuarioDto
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public long RolId { get; set; }
    public string Rol { get; set; } = string.Empty;
    public long SucursalId { get; set; }
    public string? Sucursal { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Estado { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}
