using Microsoft.EntityFrameworkCore;
using DefontanaTechnicalTest.Entities;

namespace DefontanaTechnicalTest.Contexts
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Local> Locales { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<VentaDetalle> VentaDetalles { get; set; } = null!;
        public virtual DbSet<Venta> Ventas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Local>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Local__3E34B29DF6370FC0");
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Marca__9B8F8DB2325A25B9");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Producto__9B4120E21FBD1C85");

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.MarcaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Producto__ID_Mar__52593CB8");
            });

            modelBuilder.Entity<VentaDetalle>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__VentaDet__2F0CE38B52091CC3");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.VentaDetalles)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaDeta__ID_Pr__5DCAEF64");

                entity.HasOne(d => d.Venta)
                    .WithMany(p => p.VentaDetalles)
                    .HasForeignKey(d => d.VentaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VentaDeta__ID_Ve__5CD6CB2B");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__Venta__3CD842E5A3F1C767");

                entity.HasOne(d => d.Local)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.LocalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Venta__ID_Local__571DF1D5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
