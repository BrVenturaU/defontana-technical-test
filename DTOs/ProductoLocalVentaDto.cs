namespace DefontanaTechnicalTest.DTOs
{
    public record ProductoLocalVentaDto(long LocalId, string Sucursal, long ProductoId, string Producto, string Codigo, int Ventas) 
        : ProductoLocalDto(LocalId, Sucursal, ProductoId, Producto, Codigo);
}
