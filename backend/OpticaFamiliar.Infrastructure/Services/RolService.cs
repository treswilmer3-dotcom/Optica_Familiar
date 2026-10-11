using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class RolService : IRolService
{
    private readonly AppDbContext _db;
    public RolService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<RolDto>> ListarAsync(CancellationToken ct = default) =>
        await _db.Roles.AsNoTracking().OrderBy(r => r.Nombre)
            .Select(r => new RolDto { Id = r.Id, Codigo = r.Codigo, Nombre = r.Nombre, Descripcion = r.Descripcion })
            .ToListAsync(ct);
}
