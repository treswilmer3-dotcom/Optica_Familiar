using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class RolService : IRolService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;

    public RolService(AppDbContext db, ICurrentUser current)
    {
        _db = db;
        _current = current;
    }

    /// <summary>El rol SUPERADMIN solo es visible para el propio SUPERADMIN.</summary>
    public async Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken ct = default) =>
        await _db.Roles.AsNoTracking()
            .Where(r => _current.EsSuperAdmin || r.Codigo != Roles.SuperAdmin)
            .OrderBy(r => r.Nombre)
            .Select(r => new RolDto { Id = r.Id, Codigo = r.Codigo, Nombre = r.Nombre, Descripcion = r.Descripcion })
            .ToListAsync(ct);
}
