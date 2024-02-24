using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DefontanaTechnicalTest.Entities
{
    [Table("Producto")]
    public partial class Producto
    {
        public Producto()
        {
            VentaDetalles = new HashSet<VentaDetalle>();
        }

        [Key]
        [Column("ID_Producto")]
        public long Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Codigo { get; set; } = null!;
        [Column("ID_Marca")]
        public long MarcaId { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Modelo { get; set; } = null!;
        [Column("Costo_Unitario")]
        public int CostoUnitario { get; set; }
        public virtual Marca Marca { get; set; } = null!;
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
