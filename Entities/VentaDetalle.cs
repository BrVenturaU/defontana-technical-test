using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefontanaTechnicalTest.Entities
{
    [Table("VentaDetalle")]
    public partial class VentaDetalle
    {
        [Key]
        [Column("ID_VentaDetalle")]
        public long Id { get; set; }
        [Column("Precio_Unitario")]
        public int PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int TotalLinea { get; set; }
        [Column("ID_Producto")]
        public long ProductoId { get; set; }
        [Column("ID_Venta")]
        public long VentaId { get; set; }
        public virtual Producto Producto { get; set; } = null!;
        public virtual Venta Venta { get; set; } = null!;
    }
}
