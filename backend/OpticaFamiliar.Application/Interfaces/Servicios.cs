using OpticaFamiliar.Application.DTOs;

namespace OpticaFamiliar.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);
}

public interface IEmpresaService
{
    Task<IReadOnlyList<EmpresaDto>> ListarAsync(CancellationToken ct = default);
    Task<EmpresaDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<EmpresaDto> CrearAsync(EmpresaRequest request, CancellationToken ct = default);
    Task<EmpresaDto> ActualizarAsync(long id, EmpresaRequest request, CancellationToken ct = default);
}

public interface ISucursalService
{
    Task<IReadOnlyList<SucursalDto>> ListarAsync(CancellationToken ct = default);
    Task<SucursalDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<SucursalDto> CrearAsync(SucursalRequest request, CancellationToken ct = default);
    Task<SucursalDto> ActualizarAsync(long id, SucursalRequest request, CancellationToken ct = default);
}

public interface IRolService
{
    Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken ct = default);
}

public interface IUsuarioService
{
    Task<IReadOnlyList<UsuarioDto>> ListarAsync(CancellationToken ct = default);
    Task<UsuarioDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<UsuarioDto> CrearAsync(UsuarioCreateRequest request, CancellationToken ct = default);
    Task<UsuarioDto> ActualizarAsync(long id, UsuarioUpdateRequest request, CancellationToken ct = default);
    Task CambiarPasswordAsync(long id, CambiarPasswordRequest request, CancellationToken ct = default);
}

public interface IClienteService
{
    Task<IReadOnlyList<ClienteDto>> ListarAsync(string? buscar, int pagina, int tamano, CancellationToken ct = default);
    Task<ClienteDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<ClienteDto> CrearAsync(ClienteRequest request, CancellationToken ct = default);
    Task<ClienteDto> ActualizarAsync(long id, ClienteRequest request, CancellationToken ct = default);
    Task EliminarAsync(long id, CancellationToken ct = default);
    Task<HistorialClienteDto> ObtenerHistorialAsync(long id, CancellationToken ct = default);
}

public interface IExamenVisualService
{
    Task<ExamenVisualDto> CrearAsync(ExamenVisualRequest request, CancellationToken ct = default);
    Task<ExamenVisualDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<ExamenVisualDto>> ListarPorClienteAsync(long clienteId, CancellationToken ct = default);
}

public interface IRecetaService
{
    Task<RecetaDto> CrearAsync(RecetaRequest request, CancellationToken ct = default);
    Task<RecetaDto> ObtenerAsync(long id, CancellationToken ct = default);
}

public interface IOrdenTrabajoService
{
    Task<OrdenTrabajoDto> CrearAsync(OrdenTrabajoRequest request, CancellationToken ct = default);
    Task<OrdenTrabajoDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<OrdenTrabajoDto>> ListarAsync(string? estado, CancellationToken ct = default);
    Task<OrdenTrabajoDto> CambiarEstadoAsync(long id, CambiarEstadoOrdenRequest request, CancellationToken ct = default);
}

public interface IVentaService
{
    Task<VentaDto> CrearAsync(VentaRequest request, CancellationToken ct = default);
    Task<VentaDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<VentaDto>> ListarAsync(int pagina, int tamano, CancellationToken ct = default);
    Task<VentaDto> RegistrarPagoAsync(long id, PagoRequest request, CancellationToken ct = default);
    Task<VentaDto> AnularAsync(long id, CancellationToken ct = default);
}

public interface IProductoService
{
    Task<IReadOnlyList<ProductoDto>> ListarAsync(string? buscar, CancellationToken ct = default);
    Task<ProductoDto> ObtenerAsync(long id, CancellationToken ct = default);
    Task<ProductoDto> CrearAsync(ProductoRequest request, CancellationToken ct = default);
}

/// <summary>Contrato de infraestructura usado por la capa de servicios.</summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface IJwtTokenService
{
    (string Token, DateTime ExpiraEn) Generar(long usuarioId, string username, string rolCodigo, long sucursalId, long empresaId);
}
