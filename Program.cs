using DefontanaTechnicalTest.Contexts;
using DefontanaTechnicalTest.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(configuration.GetConnectionString("Default"))
    //.LogTo(s=> Console.WriteLine(s))
    .Options;


await using var context = new AppDbContext(contextOptions);
var ventaService = new VentaService(context);

var ventas = await ventaService.ObtenerVentasNDias();
var detallesVentas = ventas.SelectMany(v => v.VentaDetalles).ToList();

Action<object, string> printResult = (value, title) =>
{
    Console.WriteLine(title);
    Console.WriteLine($"    {value}");
    Console.WriteLine();
};

Console.WriteLine();
var totalVentas = ventaService.ObtenerTotalVentas(ventas);
printResult(totalVentas, "Total de ventas de los últimos N días (monto total y cantidad total de ventas)");

var ventaMayor = ventaService.ObtenerVentaMayor(ventas);
printResult(ventaMayor, "Día y hora en que se realizó la venta con el monto más alto (y cuál es el monto).");

var productoMayorMontoVentas = ventaService.ObtenerProductoVentaMayor(detallesVentas);
printResult(productoMayorMontoVentas, "Indicar cuál es el producto con mayor monto total de ventas.");

var localMayorVenta = ventaService.ObtenerLocalMayorVenta(ventas);
printResult(localMayorVenta, "Indicar el local con mayor monto de ventas.");

var marcaMayorGanancia = ventaService.ObtenerMarcaMayorGanancia(detallesVentas);
printResult(marcaMayorGanancia, "Marca con mayor margen de ganancias");

var productosVendidosLocal = ventaService.ProductoMejorVendidoLocal(detallesVentas);
//printResult(string.Join("\r\n", productosVendidosLocal.Select(p => p.ToString())), "¿Cómo obtendrías cuál es el producto que más se vende en cada local?");
Console.WriteLine("¿Cómo obtendrías cuál es el producto que más se vende en cada local?");
for (int i = 0; i < productosVendidosLocal.Count; i++)
{
    Console.WriteLine($"    {i + 1} - {productosVendidosLocal[i]}");
}

Console.ReadKey();