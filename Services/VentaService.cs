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
                .GroupBy(vd => new ProductoDto(vd.ProductoId, vd.Producto.Nombre, vd.Producto.Codigo))
                .Select(g =>
                    new ProductoVentaMayorDto(g.Key.Id, g.Key.Nombre, g.Key.Codigo, g.Sum(p => p.TotalLinea)))
                .MaxBy(p => p.MontoTotal);

        public LocalVentaMayorDto ObtenerLocalMayorVenta(List<Venta> ventas) =>
            ventas
                .GroupBy(v => new LocalDto(v.LocalId, v.Local.Nombre))
                .Select(g =>
                    new LocalVentaMayorDto(g.Key.Id, g.Key.Nombre, g.Sum(v => v.Total))
                )
                .MaxBy(l => l.MontoVentas);

        public MarcaMayorGananciaDto ObtenerMarcaMayorGanancia(List<VentaDetalle> detalles) =>
            detalles
                .GroupBy(vd => new MarcaDto(vd.Producto.MarcaId, vd.Producto.Marca.Nombre))
                .Select(g =>
                    new MarcaMayorGananciaDto(g.Key.Id, g.Key.Nombre,
                        g.Sum(vd => vd.TotalLinea),
                        g.Sum(vd => vd.Producto.CostoUnitario * vd.Cantidad),
                        g.Sum(vd => vd.Cantidad * (vd.PrecioUnitario - vd.Producto.CostoUnitario)))
                )
                .MaxBy(m => m.Ganancia);

    public List<ProductoLocalVentaDto> ProductoMejorVendidoLocal(List<VentaDetalle> detalles)
        {
            var productosVendidosLocal = detalles
                .GroupBy(vd => new ProductoLocalDto(vd.Venta.LocalId,
                    vd.Venta.Local.Nombre,
                    vd.ProductoId,
                    vd.Producto.Nombre,
                    vd.Producto.Codigo))
                .Select(g => new ProductoLocalVentaDto(g.Key.LocalId,
                    g.Key.Sucursal,
                    g.Key.ProductoId,
                    g.Key.Producto,
                    g.Key.Codigo, g.Sum(vd => vd.Cantidad)));

            var locales = detalles
                .GroupBy(vd => new LocalDto(vd.Venta.LocalId,
                    vd.Venta.Local.Nombre))
                .Select(g => g.Key);

            return locales
                .Select(l => productosVendidosLocal
                .Where(p => p.LocalId == l.Id)
                .OrderByDescending(p => p.Ventas)
                    .ThenBy(p => p.ProductoId)
                .MaxBy(p => p.Ventas))
                .OrderBy(l => l.LocalId).ToList();
        }
    }
}
