using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var res = await _auth.LoginAsync(request, ct);
        return res == null ? Unauthorized(new ProblemDetails { Status = 401, Title = "Credenciales inválidas" }) : Ok(res);
    }
}
