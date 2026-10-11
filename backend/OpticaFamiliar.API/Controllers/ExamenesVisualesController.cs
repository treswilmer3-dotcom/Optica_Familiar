using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/examenes-visuales")]
[Authorize(Roles = Roles.Todos)]
public class ExamenesVisualesController : ControllerBase
{
    private readonly IExamenVisualService _svc;
    public ExamenesVisualesController(IExamenVisualService svc) => _svc = svc;

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ExamenVisualDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpGet("cliente/{clienteId:long}")]
    public async Task<ActionResult<IReadOnlyList<ExamenVisualDto>>> PorCliente(long clienteId, CancellationToken ct) => Ok(await _svc.ListarPorClienteAsync(clienteId, ct));

    [HttpPost, Authorize(Roles = Roles.AdminOOptometrista)]
    public async Task<ActionResult<ExamenVisualDto>> Crear(ExamenVisualRequest r, CancellationToken ct)
    {
        var e = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = e.Id }, e);
    }
}
