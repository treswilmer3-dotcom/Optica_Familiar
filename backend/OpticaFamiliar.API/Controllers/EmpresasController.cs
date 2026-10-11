using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/empresas")]
[Authorize(Roles = Roles.Administrador)]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _svc;
    public EmpresasController(IEmpresaService svc) => _svc = svc;

    [HttpGet] public async Task<ActionResult<IReadOnlyList<EmpresaDto>>> Listar(CancellationToken ct) => Ok(await _svc.ListarAsync(ct));

    [HttpGet("{id:long}")] public async Task<ActionResult<EmpresaDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<EmpresaDto>> Crear(EmpresaRequest r, CancellationToken ct)
    {
        var e = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = e.Id }, e);
    }

    [HttpPut("{id:long}")] public async Task<ActionResult<EmpresaDto>> Actualizar(long id, EmpresaRequest r, CancellationToken ct) => Ok(await _svc.ActualizarAsync(id, r, ct));
}
