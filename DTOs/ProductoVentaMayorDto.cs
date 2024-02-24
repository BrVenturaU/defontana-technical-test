namespace DefontanaTechnicalTest.DTOs
{
    public record ProductoVentaMayorDto(long Id, string Nombre, string Codigo, int MontoTotal) : ProductoDto(Id, Nombre,
        Codigo);
}
