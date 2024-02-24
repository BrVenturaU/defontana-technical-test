using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefontanaTechnicalTest.Entities
{
    [Table("Venta")]
    public partial class Venta
    {
        public Venta()
        {
            VentaDetalles = new HashSet<VentaDetalle>();
        }

        [Key]
        [Column("ID_Venta")]
        public long Id { get; set; }
        public int Total { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column("ID_Local")]
        public long LocalId { get; set; }
        public virtual Local Local { get; set; } = null!;
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
