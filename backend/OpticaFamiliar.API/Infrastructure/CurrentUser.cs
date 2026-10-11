using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Infrastructure;

/// <summary>Resuelve el usuario autenticado a partir de los claims del JWT.</summary>
public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor) => _accessor = accessor;

    private System.Security.Claims.ClaimsPrincipal Principal =>
        _accessor.HttpContext?.User ?? throw new ForbiddenException("No hay un usuario autenticado.");

    private long Claim(string tipo) =>
        long.TryParse(Principal.FindFirst(tipo)?.Value, out var v) ? v
            : throw new ForbiddenException("El token no contiene la información requerida.");

    public long UserId => Claim("sub");
    public long SucursalId => Claim("sucursal_id");
    public string Rol => Principal.FindFirst("role")?.Value ?? string.Empty;
    public bool EsAdministrador => Rol == Roles.Administrador;
}
