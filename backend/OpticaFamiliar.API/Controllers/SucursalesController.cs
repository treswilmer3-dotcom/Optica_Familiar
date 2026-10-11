using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/sucursales")]
[Authorize]
public class SucursalesController : ControllerBase
{
    private readonly ISucursalService _svc;
    public SucursalesController(ISucursalService svc) => _svc = svc;

    [HttpGet] public async Task<ActionResult<IReadOnlyList<SucursalDto>>> Listar(CancellationToken ct) => Ok(await _svc.ListarAsync(ct));

    [HttpGet("{id:long}")] public async Task<ActionResult<SucursalDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<SucursalDto>> Crear(SucursalRequest r, CancellationToken ct)
    {
        var s = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = s.Id }, s);
    }

    [HttpPut("{id:long}"), Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<SucursalDto>> Actualizar(long id, SucursalRequest r, CancellationToken ct) => Ok(await _svc.ActualizarAsync(id, r, ct));
}
