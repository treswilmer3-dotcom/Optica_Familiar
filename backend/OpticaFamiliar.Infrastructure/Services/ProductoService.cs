using Microsoft.EntityFrameworkCore;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Productos;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

/// <summary>Catálogo mínimo de productos necesario para facturar (el inventario completo queda fuera de la Fase 1).</summary>
public class ProductoService : IProductoService
{
    private readonly AppDbContext _db;
    public ProductoService(AppDbContext db) => _db = db;

    public async Task<IReadOnlyList<ProductoDto>> ListarAsync(string? buscar, CancellationToken ct = default)
    {
        var q = _db.Productos.AsNoTracking().Include(p => p.Categoria).Where(p => p.Estado == Estados.Activo);
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var t = buscar.Trim().ToLower();
            q = q.Where(p => p.Nombre.ToLower().Contains(t) || p.Codigo.ToLower().Contains(t));
        }
        return (await q.OrderBy(p => p.Nombre).Take(200).ToListAsync(ct)).Select(Map).ToList();
    }

    public async Task<ProductoDto> ObtenerAsync(long id, CancellationToken ct = default) =>
        Map(await _db.Productos.AsNoTracking().Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException("Producto no encontrado."));

    public async Task<ProductoDto> CrearAsync(ProductoRequest r, CancellationToken ct = default)
    {
        if (!await _db.CategoriaProductos.AnyAsync(c => c.Id == r.CategoriaId, ct))
            throw new BusinessRuleException("La categoría indicada no existe.");
        if (await _db.Productos.AnyAsync(p => p.Codigo == r.Codigo, ct))
            throw new BusinessRuleException("Ya existe un producto con ese código.");

        var p = new Producto
        {
            CategoriaId = r.CategoriaId, MarcaId = r.MarcaId, Codigo = r.Codigo.Trim(), CodigoBarras = r.CodigoBarras,
            Nombre = r.Nombre.Trim(), Descripcion = r.Descripcion, Costo = r.Costo, Precio = r.Precio,
            RequiereFormula = r.RequiereFormula
        };
        _db.Productos.Add(p);
        await _db.SaveChangesAsync(ct);
        return await ObtenerAsync(p.Id, ct);
    }

    public async Task<IReadOnlyList<CategoriaProductoDto>> ListarCategoriasAsync(CancellationToken ct = default) =>
        await _db.CategoriaProductos.AsNoTracking().OrderBy(c => c.Nombre)
            .Select(c => new CategoriaProductoDto { Id = c.Id, Codigo = c.Codigo, Nombre = c.Nombre })
            .ToListAsync(ct);

    private static ProductoDto Map(Producto p) => new()
    {
        Id = p.Id, CategoriaId = p.CategoriaId, Categoria = p.Categoria?.Nombre, MarcaId = p.MarcaId,
        Codigo = p.Codigo, CodigoBarras = p.CodigoBarras, Nombre = p.Nombre, Descripcion = p.Descripcion,
        Costo = p.Costo, Precio = p.Precio, RequiereFormula = p.RequiereFormula, Estado = p.Estado
    };
}
