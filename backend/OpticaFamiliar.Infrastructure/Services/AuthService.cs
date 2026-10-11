using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class AuthService : IAuthService
{
    // Hash BCrypt válido usado para igualar el tiempo de respuesta cuando el usuario no existe.
    private const string HashFicticio = "$2a$11$7EqJtq98hPqEX7fNZaFWoOhi5BcpnEhYtXkQj4aXwK6lJ5h0Zf1eG";

    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthService(AppDbContext db, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var codigo = request.CodigoEmpresa.Trim();
        var username = request.Username.Trim();

        // El login ocurre antes de conocer la empresa: se saltan los filtros y se acota por código de empresa.
        var empresa = await _db.Empresas.IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Codigo == codigo && e.Estado == Estados.Activo, ct);

        var usuario = empresa == null ? null : await _db.Usuarios.IgnoreQueryFilters()
            .Include(u => u.Rol)
            .Include(u => u.Sucursal)
            .FirstOrDefaultAsync(u => u.EmpresaId == empresa.Id && u.Username == username, ct);

        if (usuario == null)
        {
            _hasher.Verify(request.Password, HashFicticio);
            return null;
        }

        if (!_hasher.Verify(request.Password, usuario.PasswordHash) || usuario.Estado != Estados.Activo)
            return null;

        var (token, expira) = _jwt.Generar(usuario.Id, usuario.Username, usuario.Rol.Codigo,
            usuario.SucursalId, usuario.EmpresaId);

        usuario.UltimoAcceso = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return new LoginResponse
        {
            Token = token,
            ExpiraEn = expira,
            UsuarioId = usuario.Id,
            Username = usuario.Username,
            Rol = usuario.Rol.Codigo,
            SucursalId = usuario.SucursalId,
            EmpresaId = usuario.EmpresaId,
            Empresa = empresa!.NombreComercial
        };
    }
}
