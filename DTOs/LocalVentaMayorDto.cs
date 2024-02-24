namespace DefontanaTechnicalTest.DTOs
{
    public record LocalVentaMayorDto(long Id, string Nombre, int MontoVentas) : LocalDto(Id, Nombre);
}
