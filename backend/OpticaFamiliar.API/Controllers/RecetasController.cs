using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/recetas")]
[Authorize(Roles = Roles.Todos)]
public class RecetasController : ControllerBase
{
    private readonly IRecetaService _svc;
    public RecetasController(IRecetaService svc) => _svc = svc;

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RecetaDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.AdminOOptometrista)]
    public async Task<ActionResult<RecetaDto>> Crear(RecetaRequest r, CancellationToken ct)
    {
        var rec = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = rec.Id }, rec);
    }
}
