using System.Security.Claims;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Infrastructure;

/// <summary>Resuelve el usuario y la empresa de la petición a partir de los claims del JWT.</summary>
public sealed class CurrentUser : ICurrentUser, ITenantContext
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    private long? ClaimNumerico(string tipo) =>
        long.TryParse(Principal?.FindFirst(tipo)?.Value, out var v) ? v : null;

    private long Requerido(string tipo) =>
        ClaimNumerico(tipo) ?? throw new ForbiddenException("El token no contiene la información requerida.");

    public long UserId => Requerido("sub");
    public long EmpresaId => Requerido("empresa_id");
    public long SucursalId => Requerido("sucursal_id");
    public string Rol => Principal?.FindFirst("role")?.Value ?? string.Empty;
    public bool EsAdministrador => Rol == Roles.Administrador;
    public bool EsSuperAdmin => Rol == Roles.SuperAdmin;

    // ITenantContext: nulo si no hay usuario autenticado (login, seed, migraciones).
    long? ITenantContext.EmpresaActual => Principal?.Identity?.IsAuthenticated == true ? ClaimNumerico("empresa_id") : null;
}
