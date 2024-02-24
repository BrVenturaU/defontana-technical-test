namespace DefontanaTechnicalTest.DTOs
{
    public record MarcaMayorGananciaDto(long Id, string Nombre, int Ventas, int Costos, int Ganancia) : MarcaDto(Id, Nombre);
}
