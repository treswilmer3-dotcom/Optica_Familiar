namespace OpticaFamiliar.Application.Interfaces;

/// <summary>Usuario autenticado de la petición actual (extraído del JWT).</summary>
public interface ICurrentUser
{
    long UserId { get; }
    long SucursalId { get; }
    string Rol { get; }
    bool EsAdministrador { get; }
}
