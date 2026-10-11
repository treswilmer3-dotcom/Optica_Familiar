using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/empresas")]
[Authorize]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _svc;
    public EmpresasController(IEmpresaService svc) => _svc = svc;

    /// <summary>Empresa del usuario autenticado (datos de configuración: moneda, IVA, país...).</summary>
    [HttpGet("actual")]
    public async Task<ActionResult<EmpresaDto>> Actual(CancellationToken ct) => Ok(await _svc.ObtenerActualAsync(ct));

    /// <summary>Identidad visual (logo y colores) de la empresa del usuario.</summary>
    [HttpGet("actual/marca")]
    public async Task<ActionResult<MarcaDto>> Marca(CancellationToken ct) => Ok(await _svc.ObtenerMarcaAsync(ct));

    [HttpPut("actual/marca"), Authorize(Roles = Roles.Administrador)]
    public async Task<ActionResult<MarcaDto>> ActualizarMarca(MarcaRequest r, CancellationToken ct) => Ok(await _svc.ActualizarMarcaAsync(r, ct));

    [HttpGet, Authorize(Roles = Roles.SuperAdmin)]
    public async Task<ActionResult<IReadOnlyList<EmpresaDto>>> Listar(CancellationToken ct) => Ok(await _svc.ListarAsync(ct));

    [HttpGet("{id:long}"), Authorize(Roles = Roles.SuperAdmin)]
    public async Task<ActionResult<EmpresaDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.SuperAdmin)]
    public async Task<ActionResult<EmpresaDto>> Crear(EmpresaRequest r, CancellationToken ct)
    {
        var e = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = e.Id }, e);
    }

    [HttpPut("{id:long}"), Authorize(Roles = Roles.SuperAdmin)]
    public async Task<ActionResult<EmpresaDto>> Actualizar(long id, EmpresaRequest r, CancellationToken ct) => Ok(await _svc.ActualizarAsync(id, r, ct));
}
