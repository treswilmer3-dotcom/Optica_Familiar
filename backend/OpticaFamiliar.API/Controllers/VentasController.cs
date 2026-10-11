using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/ventas")]
[Authorize(Roles = Roles.AdminOVendedor)]
public class VentasController : ControllerBase
{
    private readonly IVentaService _svc;
    public VentasController(IVentaService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VentaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamano = 20, CancellationToken ct = default) =>
        Ok(await _svc.ListarAsync(pagina, tamano, ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<VentaDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<VentaDto>> Crear(VentaRequest r, CancellationToken ct)
    {
        var v = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = v.Id }, v);
    }

    [HttpPost("{id:long}/pagos")]
    public async Task<ActionResult<VentaDto>> RegistrarPago(long id, PagoRequest r, CancellationToken ct) => Ok(await _svc.RegistrarPagoAsync(id, r, ct));

    [HttpPost("{id:long}/anular"), Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<VentaDto>> Anular(long id, CancellationToken ct) => Ok(await _svc.AnularAsync(id, ct));
}
