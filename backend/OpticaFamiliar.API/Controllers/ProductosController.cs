using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/productos")]
[Authorize(Roles = Roles.Todos)]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _svc;
    public ProductosController(IProductoService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductoDto>>> Listar([FromQuery] string? buscar, CancellationToken ct) => Ok(await _svc.ListarAsync(buscar, ct));

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProductoDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoRequest r, CancellationToken ct)
    {
        var p = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = p.Id }, p);
    }
}
