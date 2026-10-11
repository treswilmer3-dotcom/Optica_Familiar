using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpticaFamiliar.Application.Interfaces;

namespace OpticaFamiliar.Infrastructure.Security;

public class JwtOptions
{
    public const string Seccion = "Jwt";
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "OpticaFamiliar";
    public string Audience { get; set; } = "OpticaFamiliarUsers";
    public int ExpiraMinutos { get; set; } = 480;
}

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options) => _options = options.Value;

    public (string Token, DateTime ExpiraEn) Generar(long usuarioId, string username, string rolCodigo, long sucursalId, long empresaId)
    {
        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var expira = DateTime.UtcNow.AddMinutes(_options.ExpiraMinutos);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim("role", rolCodigo),
            new Claim("sucursal_id", sucursalId.ToString()),
            new Claim("empresa_id", empresaId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expira,
            signingCredentials: new SigningCredentials(clave, SecurityAlgorithms.HmacSha256));

        return (new JwtSecurityTokenHandler().WriteToken(token), expira);
    }
}
