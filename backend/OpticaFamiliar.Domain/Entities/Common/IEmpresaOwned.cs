namespace OpticaFamiliar.Domain.Entities.Common
{
    /// <summary>
    /// Entidad perteneciente a una empresa (tenant). Se filtra automáticamente por empresa en el DbContext
    /// y se asigna al insertar.
    /// </summary>
    public interface IEmpresaOwned
    {
        long EmpresaId { get; set; }
    }
}
