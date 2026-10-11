using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Organizacion;
using OpticaFamiliar.Infrastructure.Data;
using OpticaFamiliar.Infrastructure.Seed;

namespace OpticaFamiliar.Infrastructure.Services;

/// <summary>Gestión de empresas (tenants). Las operaciones globales son exclusivas del SUPERADMIN (ver controller).</summary>
public class EmpresaService : IEmpresaService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;

    public EmpresaService(AppDbContext db, ICurrentUser current)
    {
        _db = db;
        _current = current;
    }

    public async Task<IReadOnlyList<EmpresaDto>> ListarAsync(CancellationToken ct = default) =>
        (await _db.Empresas.IgnoreQueryFilters().AsNoTracking().OrderBy(e => e.NombreComercial).ToListAsync(ct)).Select(Map).ToList();

    public async Task<EmpresaDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.Empresas.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException("Empresa no encontrada."));

    public async Task<EmpresaDto> ObtenerActualAsync(CancellationToken ct = default) =>
        Map(await _db.Empresas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == _current.EmpresaId, ct)
            ?? throw new NotFoundException("Empresa no encontrada."));

    public async Task<EmpresaDto> CrearAsync(EmpresaRequest r, CancellationToken ct = default)
    {
        await ValidarUnicidad(null, r, ct);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var e = new Empresa();
        Aplicar(e, r);
        _db.Empresas.Add(e);
        await _db.SaveChangesAsync(ct);

        _db.CategoriaProductos.AddRange(Aprovisionamiento.CategoriasBase(e.Id));
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
        return Map(e);
    }

    public async Task<EmpresaDto> ActualizarAsync(long id, EmpresaRequest r, CancellationToken ct = default)
    {
        var e = await _db.Empresas.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Empresa no encontrada.");
        await ValidarUnicidad(id, r, ct);
        if (id == _current.EmpresaId && r.Estado == Estados.Inactivo)
            throw new BusinessRuleException("No se puede desactivar la empresa de la plataforma.");

        Aplicar(e, r);
        await _db.SaveChangesAsync(ct);
        return Map(e);
    }

    private async Task ValidarUnicidad(long? id, EmpresaRequest r, CancellationToken ct)
    {
        var codigo = r.Codigo.Trim();
        var fiscal = r.IdentificacionFiscal.Trim();
        var pais = r.Pais.Trim().ToUpper();
        if (await _db.Empresas.IgnoreQueryFilters().AnyAsync(e => e.Id != id &&
                (e.Codigo == codigo || (e.Pais == pais && e.IdentificacionFiscal == fiscal)), ct))
            throw new BusinessRuleException("Ya existe una empresa con ese código o identificación fiscal.");
        if (r.Estado != null && r.Estado != Estados.Activo && r.Estado != Estados.Inactivo)
            throw new BusinessRuleException("Estado inválido (ACTIVO / INACTIVO).");
    }

    private static void Aplicar(Empresa e, EmpresaRequest r)
    {
        e.Codigo = r.Codigo.Trim();
        e.RazonSocial = r.RazonSocial.Trim();
        e.NombreComercial = r.NombreComercial.Trim();
        e.IdentificacionFiscal = r.IdentificacionFiscal.Trim();
        e.Pais = r.Pais.Trim().ToUpper();
        e.Moneda = r.Moneda.Trim().ToUpper();
        e.ZonaHoraria = r.ZonaHoraria.Trim();
        e.Idioma = r.Idioma.Trim().ToLower();
        e.IvaPorcentaje = r.IvaPorcentaje;
        e.Direccion = r.Direccion;
        e.Telefono = r.Telefono;
        e.Correo = r.Correo;
        e.SitioWeb = r.SitioWeb;
        if (r.Estado != null) e.Estado = r.Estado;
    }

    private static EmpresaDto Map(Empresa e) => new()
    {
        Id = e.Id, Codigo = e.Codigo, RazonSocial = e.RazonSocial, NombreComercial = e.NombreComercial,
        IdentificacionFiscal = e.IdentificacionFiscal, Pais = e.Pais, Moneda = e.Moneda,
        ZonaHoraria = e.ZonaHoraria, Idioma = e.Idioma, IvaPorcentaje = e.IvaPorcentaje,
        Direccion = e.Direccion, Telefono = e.Telefono, Correo = e.Correo, SitioWeb = e.SitioWeb, Estado = e.Estado
    };
}
