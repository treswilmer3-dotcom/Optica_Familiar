using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/ordenes-trabajo")]
[Authorize(Roles = Roles.Todos)]
public class OrdenesTrabajoController : ControllerBase
{
    private readonly IOrdenTrabajoService _svc;
    public OrdenesTrabajoController(IOrdenTrabajoService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrdenTrabajoDto>>> Listar([FromQuery] string? estado, CancellationToken ct) => Ok(await _svc.ListarAsync(estado, ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrdenTrabajoDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.AdminOVendedor)]
    public async Task<ActionResult<OrdenTrabajoDto>> Crear(OrdenTrabajoRequest r, CancellationToken ct)
    {
        var o = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = o.Id }, o);
    }

    [HttpPatch("{id:long}/estado"), Authorize(Roles = Roles.AdminOVendedor)]
    public async Task<ActionResult<OrdenTrabajoDto>> CambiarEstado(long id, CambiarEstadoOrdenRequest r, CancellationToken ct) => Ok(await _svc.CambiarEstadoAsync(id, r, ct));
}
