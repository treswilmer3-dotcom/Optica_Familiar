using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _svc;
    public UsuariosController(IUsuarioService svc) => _svc = svc;

    [HttpGet, Authorize(Roles = Roles.AdminOSuper)]
    public async Task<ActionResult<IReadOnlyList<UsuarioDto>>> Listar(CancellationToken ct) => Ok(await _svc.ListarAsync(ct));

    [HttpGet("{id:long}"), Authorize(Roles = Roles.AdminOSuper)]
    public async Task<ActionResult<UsuarioDto>> Obtener(long id, CancellationToken ct) => Ok(await _svc.ObtenerAsync(id, ct));

    [HttpPost, Authorize(Roles = Roles.AdminOSuper)]
    public async Task<ActionResult<UsuarioDto>> Crear(UsuarioCreateRequest r, CancellationToken ct)
    {
        var u = await _svc.CrearAsync(r, ct);
        return CreatedAtAction(nameof(Obtener), new { id = u.Id }, u);
    }

    [HttpPut("{id:long}"), Authorize(Roles = Roles.AdminOSuper)]
    public async Task<ActionResult<UsuarioDto>> Actualizar(long id, UsuarioUpdateRequest r, CancellationToken ct) => Ok(await _svc.ActualizarAsync(id, r, ct));

    /// <summary>Cada usuario cambia su contraseña; un administrador puede restablecer la de otros.</summary>
    [HttpPut("{id:long}/password")]
    public async Task<IActionResult> CambiarPassword(long id, CambiarPasswordRequest r, CancellationToken ct)
    {
        await _svc.CambiarPasswordAsync(id, r, ct);
        return NoContent();
    }
}
