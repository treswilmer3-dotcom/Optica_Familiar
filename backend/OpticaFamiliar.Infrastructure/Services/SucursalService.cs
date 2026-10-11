using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class SucursalService : ISucursalService
{
    private readonly AppDbContext _db;
    public SucursalService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<SucursalDto>> ListarAsync(CancellationToken ct = default) =>
        (await _db.Sucursales.AsNoTracking().OrderBy(s => s.Nombre).ToListAsync(ct)).Select(Map).ToList();

    public async Task<SucursalDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.Sucursales.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new NotFoundException("Sucursal no encontrada."));

    public async Task<SucursalDto> CrearAsync(SucursalRequest r, CancellationToken ct = default)
    {
        await Validar(null, r, ct);
        var s = new Sucursal();
        Aplicar(s, r);
        _db.Sucursales.Add(s);
        await _db.SaveChangesAsync(ct);
        return Map(s);
    }

    public async Task<SucursalDto> ActualizarAsync(long id, SucursalRequest r, CancellationToken ct = default)
    {
        var s = await _db.Sucursales.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Sucursal no encontrada.");
        await Validar(id, r, ct);
        Aplicar(s, r);
        await _db.SaveChangesAsync(ct);
        return Map(s);
    }

    private async Task Validar(long? id, SucursalRequest r, CancellationToken ct)
    {
        if (!await _db.Empresas.AnyAsync(e => e.Id == r.EmpresaId, ct))
            throw new BusinessRuleException("La empresa indicada no existe.");
        if (await _db.Sucursales.AnyAsync(s => s.Id != id && s.EmpresaId == r.EmpresaId && s.Codigo == r.Codigo, ct))
            throw new BusinessRuleException("Ya existe una sucursal con ese código en la empresa.");
    }

    private static void Aplicar(Sucursal s, SucursalRequest r)
    {
        s.EmpresaId = r.EmpresaId;
        s.Codigo = r.Codigo.Trim();
        s.Nombre = r.Nombre.Trim();
        s.Direccion = r.Direccion;
        s.Telefono = r.Telefono;
        s.Correo = r.Correo;
        s.Ciudad = r.Ciudad;
        s.Provincia = r.Provincia;
    }

    private static SucursalDto Map(Sucursal s) => new()
    {
        Id = s.Id, EmpresaId = s.EmpresaId, Codigo = s.Codigo, Nombre = s.Nombre, Direccion = s.Direccion,
        Telefono = s.Telefono, Correo = s.Correo, Ciudad = s.Ciudad, Provincia = s.Provincia, Estado = s.Estado
    };
}
