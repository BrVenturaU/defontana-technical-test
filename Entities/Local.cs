using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DefontanaTechnicalTest.Entities
{
    [Table("Local")]
    public partial class Local
    {
        public Local()
        {
            Ventas = new HashSet<Venta>();
        }

        [Key]
        [Column("ID_Local")]
        public long Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Direccion { get; set; } = null!;

        [InverseProperty("Local")]
        public virtual ICollection<Venta> Ventas { get; set; }
    }
}
