using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpticaFamiliar.Application.Common;
using OpticaFamiliar.Application.DTOs;
using OpticaFamiliar.Application.Interfaces;
using OpticaFamiliar.Domain.Entities.Pagos;
using OpticaFamiliar.Domain.Entities.Ventas;
using OpticaFamiliar.Infrastructure.Data;

namespace OpticaFamiliar.Infrastructure.Services;

public class VentaService : IVentaService
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _current;
    private readonly NegocioOptions _negocio;
    private readonly NumeradorDocumentos _numerador;

    public VentaService(AppDbContext db, ICurrentUser current, IOptions<NegocioOptions> negocio)
    {
        _db = db;
        _current = current;
        _negocio = negocio.Value;
        _numerador = new NumeradorDocumentos(db);
    }

    private IQueryable<Venta> Consulta() => _db.Ventas
        .Include(v => v.Cliente).ThenInclude(c => c.Persona)
        .Include(v => v.VentaDetalles).ThenInclude(d => d.Producto)
        .Include(v => v.Pagos);

    /// <summary>Los usuarios no administradores solo ven ventas de su sucursal.</summary>
    private IQueryable<Venta> Visibles(IQueryable<Venta> q) =>
        _current.EsAdministrador ? q : q.Where(v => v.SucursalId == _current.SucursalId);

    public async Task<VentaDto> CrearAsync(VentaRequest r, CancellationToken ct = default)
    {
        var cliente = await _db.Clientes.Include(c => c.Persona)
            .FirstOrDefaultAsync(c => c.Id == r.ClienteId && c.Persona.Estado == Estados.Activo, ct)
            ?? throw new BusinessRuleException("El cliente indicado no existe.");

        var ids = r.Detalles.Select(d => d.ProductoId).Distinct().ToList();
        var productos = await _db.Productos.Where(p => ids.Contains(p.Id) && p.Estado == Estados.Activo)
            .ToDictionaryAsync(p => p.Id, ct);
        if (productos.Count != ids.Count)
            throw new BusinessRuleException("Uno o más productos no existen o están inactivos.");

        Domain.Entities.OrdenesTrabajo.OrdenTrabajo? orden = null;
        if (r.OrdenTrabajoId.HasValue)
        {
            var o = await _db.OrdenesTrabajo.FirstOrDefaultAsync(x => x.Id == r.OrdenTrabajoId, ct)
                ?? throw new BusinessRuleException("La orden de trabajo indicada no existe.");
            if (o.VentaId != null) throw new BusinessRuleException("La orden de trabajo ya está vinculada a otra venta.");
            orden = o;
        }

        var detalles = new List<VentaDetalle>();
        foreach (var d in r.Detalles)
        {
            var precio = Math.Round(d.PrecioUnitario ?? productos[d.ProductoId].Precio, 2);
            var bruto = precio * d.Cantidad;
            if (d.Descuento > bruto)
                throw new BusinessRuleException("El descuento de una línea no puede superar su valor.");
            detalles.Add(new VentaDetalle
            {
                ProductoId = d.ProductoId, Cantidad = d.Cantidad, PrecioUnitario = precio,
                Descuento = Math.Round(d.Descuento, 2), Subtotal = Math.Round(bruto - d.Descuento, 2)
            });
        }

        // Subtotal = base imponible (ya con descuentos de línea); Descuento = suma informativa de descuentos.
        var subtotal = detalles.Sum(d => d.Subtotal);
        var iva = Math.Round(subtotal * _negocio.IvaPorcentaje / 100m, 2);
        var total = subtotal + iva;
        var pagado = Math.Round(r.Pagos.Sum(p => p.Valor), 2);
        if (pagado > total) throw new BusinessRuleException("Los pagos superan el total de la venta.");

        await using var tx = await _db.Database.BeginTransactionAsync(ct);
        var venta = new Venta
        {
            ClienteId = cliente.Id,
            UsuarioId = _current.UserId,
            SucursalId = _current.SucursalId,
            NumeroFactura = await _numerador.SiguienteAsync(_current.SucursalId, TiposDocumento.Venta, "001", ct),
            FechaVenta = DateTime.UtcNow,
            Subtotal = subtotal,
            Descuento = detalles.Sum(d => d.Descuento),
            Iva = iva,
            Total = total,
            Estado = pagado == total ? Estados.VentaPagada : Estados.VentaPendiente,
            VentaDetalles = detalles,
            Pagos = r.Pagos.Select(p => NuevoPago(p)).ToList()
        };
        _db.Ventas.Add(venta);
        if (orden != null) orden.Venta = venta;
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return await ObtenerAsync(venta.Id, ct);
    }

    public async Task<VentaDto> ObtenerAsync(long id, CancellationToken ct = default)
    {
        var v = await Consulta().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Venta no encontrada.");
        if (!_current.EsAdministrador && v.SucursalId != _current.SucursalId)
            throw new ForbiddenException("La venta pertenece a otra sucursal.");
        return Map(v);
    }

    public async Task<IReadOnlyList<VentaDto>> ListarAsync(int pagina, int tamano, CancellationToken ct = default)
    {
        pagina = Math.Max(pagina, 1);
        tamano = Math.Clamp(tamano, 1, 100);
        var lista = await Visibles(Consulta().AsNoTracking()).OrderByDescending(v => v.FechaVenta)
            .Skip((pagina - 1) * tamano).Take(tamano).ToListAsync(ct);
        return lista.Select(Map).ToList();
    }

    public async Task<VentaDto> RegistrarPagoAsync(long id, PagoRequest r, CancellationToken ct = default)
    {
        var v = await Visibles(_db.Ventas.Include(x => x.Pagos)).FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Venta no encontrada.");
        if (v.Estado == Estados.VentaAnulada) throw new BusinessRuleException("La venta está anulada.");

        var pendiente = v.Total - v.Pagos.Sum(p => p.Valor);
        if (r.Valor > pendiente) throw new BusinessRuleException($"El pago supera el saldo pendiente ({pendiente:0.00}).");

        v.Pagos.Add(NuevoPago(r));
        if (r.Valor == pendiente) v.Estado = Estados.VentaPagada;
        await _db.SaveChangesAsync(ct);
        return await ObtenerAsync(id, ct);
    }

    public async Task<VentaDto> AnularAsync(long id, CancellationToken ct = default)
    {
        var v = await Visibles(_db.Ventas).FirstOrDefaultAsync(x => x.Id == id, ct)
            ?? throw new NotFoundException("Venta no encontrada.");
        if (v.Estado == Estados.VentaAnulada) throw new BusinessRuleException("La venta ya está anulada.");
        v.Estado = Estados.VentaAnulada;
        await _db.SaveChangesAsync(ct);
        return await ObtenerAsync(id, ct);
    }

    private static Pago NuevoPago(PagoRequest p) => new()
    {
        MetodoPago = p.MetodoPago.Trim().ToUpper(), Valor = Math.Round(p.Valor, 2),
        Referencia = p.Referencia, FechaPago = DateTime.UtcNow
    };

    private static VentaDto Map(Venta v) => new()
    {
        Id = v.Id, ClienteId = v.ClienteId,
        ClienteNombre = $"{v.Cliente.Persona.Nombres} {v.Cliente.Persona.Apellidos}".Trim(),
        UsuarioId = v.UsuarioId, SucursalId = v.SucursalId, NumeroFactura = v.NumeroFactura,
        FechaVenta = v.FechaVenta, Subtotal = v.Subtotal, Descuento = v.Descuento, Iva = v.Iva, Total = v.Total,
        TotalPagado = v.Pagos.Sum(p => p.Valor), Estado = v.Estado,
        Detalles = v.VentaDetalles.Select(d => new VentaDetalleDto
        {
            Id = d.Id, ProductoId = d.ProductoId, Producto = d.Producto?.Nombre, Cantidad = d.Cantidad,
            PrecioUnitario = d.PrecioUnitario, Descuento = d.Descuento, Subtotal = d.Subtotal
        }).ToList(),
        Pagos = v.Pagos.Select(p => new PagoDto
        {
            Id = p.Id, MetodoPago = p.MetodoPago, Valor = p.Valor, Referencia = p.Referencia, FechaPago = p.FechaPago
        }).ToList()
    };
}
