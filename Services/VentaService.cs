using DefontanaTechnicalTest.Contexts;
using DefontanaTechnicalTest.DTOs;
using DefontanaTechnicalTest.Entities;
using Microsoft.EntityFrameworkCore;

namespace DefontanaTechnicalTest.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<Venta>> ObtenerVentasNDias(int dias = 30)
        {
            var hoy = DateTime.UtcNow;
            var fechaInicio = hoy.AddDays(-dias);
            var fechaFin = hoy;

            return _context.Ventas
                .Include(v => v.Local)
                .Include(v => v.VentaDetalles)
                .ThenInclude(vd => vd.Producto)
                .ThenInclude(p => p.Marca)
                .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                .AsNoTracking()
                .ToListAsync();

        }

        public TotalVentasDto ObtenerTotalVentas(List<Venta> ventas) => 
            new TotalVentasDto(ventas.Sum(d => d.Total), ventas.Count);

        public VentaDto ObtenerVentaMayor(List<Venta> ventas)
        {
            var ventaMayor = ventas.MaxBy(v => v.Total);
            return new VentaDto(ventaMayor.Id, ventaMayor.Fecha.ToString("F"), ventaMayor.Total);
        }

        public ProductoVentaMayorDto ObtenerProductoVentaMayor(List<VentaDetalle> detalles) =>
            detalles
                .Select(vd => new
                {
                    ProductoId = vd.Producto.Id,
                    vd.Producto.Nombre,
                    vd.Producto.Codigo,
                    vd.TotalLinea
                })
                .GroupBy(p => new ProductoDto(p.ProductoId, p.Nombre, p.Codigo))
                .Select(g =>
                    new ProductoVentaMayorDto(g.Key.Id, g.Key.Nombre, g.Key.Codigo, g.Sum(p => p.TotalLinea)))
                .MaxBy(p => p.MontoTotal);

        public LocalVentaMayorDto ObtenerLocalMayorVenta(List<Venta> ventas) =>
            ventas
                .Select(d => new
                {
                    d.LocalId,
                    d.Local.Nombre,
                    d.Total
                })
                .GroupBy(l => new LocalDto(l.LocalId, l.Nombre))
                .Select(g =>
                    new LocalVentaMayorDto(g.Key.Id, g.Key.Nombre, g.Sum(v => v.Total))
                )
                .MaxBy(l => l.MontoVentas);

        public MarcaMayorGananciaDto ObtenerMarcaMayorGanancia(List<VentaDetalle> detalles) =>
            detalles
                .Select(vd => new
                {
                    vd.Producto.MarcaId,
                    Marca = vd.Producto.Marca.Nombre,
                    Venta = vd.TotalLinea,
                    Costo = vd.Producto.CostoUnitario * vd.Cantidad,
                    MargenGanancia = vd.Cantidad * (vd.PrecioUnitario - vd.Producto.CostoUnitario)
                }).GroupBy(m => new MarcaDto(m.MarcaId, m.Marca))
                .Select(g =>
                    new MarcaMayorGananciaDto(g.Key.Id, g.Key.Nombre, g.Sum(m => m.Venta), g.Sum(m => m.Costo), g.Sum(m => m.MargenGanancia)))
                .MaxBy(m => m.Ganancia);

        public List<ProductoLocalVentaDto> ProductoMejorVendidoLocal(List<VentaDetalle> detalles)
        {
            var productosVendidosLocal = detalles
                .Select(vd => new
                {
                    vd.Venta.LocalId,
                    Sucursal = vd.Venta.Local.Nombre,
                    vd.ProductoId,
                    Producto = vd.Producto.Nombre,
                    vd.Producto.Codigo,
                    vd.Cantidad
                }).GroupBy(l => new ProductoLocalDto(l.LocalId,
                    l.Sucursal,
                    l.ProductoId,
                    l.Producto,
                    l.Codigo))
                .Select(g => new ProductoLocalVentaDto(g.Key.LocalId,
                    g.Key.Sucursal,
                    g.Key.ProductoId,
                    g.Key.Producto,
                    g.Key.Codigo, g.Sum(g => g.Cantidad)));

            var locales = productosVendidosLocal
                .GroupBy(l => new LocalDto(l.LocalId,
                    l.Sucursal))
                .Select(g => new LocalDto(g.Key.Id,
                    g.Key.Nombre));

            return locales.Select(l => productosVendidosLocal
                    .Where(p => p.LocalId == l.Id)
                    .OrderByDescending(p => p.Ventas)
                    .ThenBy(p => p.ProductoId)
                    .MaxBy(p => p.Ventas))
                .OrderBy(l => l.LocalId).ToList();
        }
    }
}
