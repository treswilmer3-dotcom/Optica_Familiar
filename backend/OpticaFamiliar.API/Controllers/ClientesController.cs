using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize(Roles = Roles.Todos)]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _svc;
    public ClientesController(IClienteService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteDto>>> Listar([FromQuery] string? buscar, [FromQuery] int pagina = 1, [FromQuery] int tamano = 20, CancellationToken ct = default) =>
        Ok(await _svc.ListarAsync(buscar, pagina, tamano, ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ClienteDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpGet("{id:long}/historial")]
    public async Task<ActionResult<HistorialClienteDto>> Historial(long id, CancellationToken ct) => Ok(await _svc.ObtenerHistorialAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.AdminOVendedor)]
    public async Task<ActionResult<ClienteDto>> Crear(ClienteRequest r, CancellationToken ct)
    {
        var c = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = c.Id }, c);
    }

    [HttpPut("{id:long}"), Authorize(Roles = Roles.AdminOVendedor)]
    public async Task<ActionResult<ClienteDto>> Actualizar(long id, ClienteRequest r, CancellationToken ct) => Ok(await _svc.ActualizarAsync(id, r, ct));

    [HttpDelete("{id:long}"), Authorize(Roles = Roles.Administrador)]
    public async Task<IActionResult> Eliminar(long id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, ct);
        return NoContent();
    }
}
