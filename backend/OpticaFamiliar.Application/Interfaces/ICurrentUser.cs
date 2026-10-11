namespace OpticaFamiliar.Application.Interfaces;

/// <summary>Usuario autenticado de la petición actual (extraído del JWT).</summary>
public interface ICurrentUser
{
    long UserId { get; }
    long EmpresaId { get; }
    long SucursalId { get; }
    string Rol { get; }
    bool EsAdministrador { get; }
    bool EsSuperAdmin { get; }
}

/// <summary>
/// Empresa (tenant) activa para el DbContext. Devuelve null fuera de una petición autenticada
/// (login, seed, migraciones): en ese caso las consultas filtradas no devuelven nada (falla cerrado).
/// </summary>
public interface ITenantContext
{
    long? EmpresaActual { get; }
    bool EsSuperAdmin { get; }
}
