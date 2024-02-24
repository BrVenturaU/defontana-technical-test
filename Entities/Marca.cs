using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DefontanaTechnicalTest.Entities
{
    [Table("Marca")]
    public partial class Marca
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        [Key]
        [Column("ID_Marca")]
        public long Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
