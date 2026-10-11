using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize(Roles = Roles.Administrador)]
public class RolesController : ControllerBase
{
    private readonly IRolService _svc;
    public RolesController(IRolService svc) => _svc = svc;

    [HttpGet] public async Task<ActionResult<IReadOnlyList<RolDto>>> Listar(CancellationToken ct) => Ok(await _svc.ListarAsync(ct));
}
