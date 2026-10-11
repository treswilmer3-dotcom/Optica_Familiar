using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class EmpresaService : IEmpresaService
{
    private readonly AppDbContext _db;
    public EmpresaService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<EmpresaDto>> ListarAsync(CancellationToken ct = default) =>
        (await _db.Empresas.AsNoTracking().OrderBy(e => e.NombreComercial).ToListAsync(ct)).Select(Map).ToList();

    public async Task<EmpresaDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.Empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException("Empresa no encontrada."));

    public async Task<EmpresaDto> CrearAsync(EmpresaRequest r, CancellationToken ct = default)
    {
        if (await _db.Empresas.AnyAsync(e => e.Ruc == r.Ruc || e.Codigo == r.Codigo, ct))
            throw new BusinessRuleException("Ya existe una empresa con ese código o RUC.");

        var e = new Empresa();
        Aplicar(e, r);
        _db.Empresas.Add(e);
        await _db.SaveChangesAsync(ct);
        return Map(e);
    }

    public async Task<EmpresaDto> ActualizarAsync(long id, EmpresaRequest r, CancellationToken ct = default)
    {
        var e = await _db.Empresas.FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Empresa no encontrada.");
        if (await _db.Empresas.AnyAsync(x => x.Id != id && (x.Ruc == r.Ruc || x.Codigo == r.Codigo), ct))
            throw new BusinessRuleException("Ya existe otra empresa con ese código o RUC.");

        Aplicar(e, r);
        await _db.SaveChangesAsync(ct);
        return Map(e);
    }

    private static void Aplicar(Empresa e, EmpresaRequest r)
    {
        e.Codigo = r.Codigo.Trim();
        e.RazonSocial = r.RazonSocial.Trim();
        e.NombreComercial = r.NombreComercial.Trim();
        e.Ruc = r.Ruc.Trim();
        e.Direccion = r.Direccion;
        e.Telefono = r.Telefono;
        e.Correo = r.Correo;
        e.SitioWeb = r.SitioWeb;
    }

    private static EmpresaDto Map(Empresa e) => new()
    {
        Id = e.Id, Codigo = e.Codigo, RazonSocial = e.RazonSocial, NombreComercial = e.NombreComercial,
        Ruc = e.Ruc, Direccion = e.Direccion, Telefono = e.Telefono, Correo = e.Correo,
        SitioWeb = e.SitioWeb, Estado = e.Estado
    };
}
