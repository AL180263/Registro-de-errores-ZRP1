namespace Test_Entities.folder
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Prueba4 : DbContext
    {
        public Prueba4()
            : base("name=Prueba4")
        {
        }

        public virtual DbSet<Lote> Lotes { get; set; }
        public virtual DbSet<Ordene> Ordenes { get; set; }
        public virtual DbSet<Resina> Resinas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<LotesAndOrdene> LotesAndOrdenes { get; set; }
        public virtual DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lote>()
                .Property(e => e.LoteNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Lote>()
                .Property(e => e.MidResina)
                .IsUnicode(false);

            modelBuilder.Entity<Lote>()
                .HasMany(e => e.LotesAndOrdenes)
                .WithRequired(e => e.Lote)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ordene>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<Ordene>()
                .Property(e => e.ResinaMid)
                .IsUnicode(false);

            modelBuilder.Entity<Ordene>()
                .HasMany(e => e.LotesAndOrdenes)
                .WithRequired(e => e.Ordene)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resina>()
                .Property(e => e.MId)
                .IsUnicode(false);

            modelBuilder.Entity<Resina>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<Resina>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Resina>()
                .HasMany(e => e.Lotes)
                .WithRequired(e => e.Resina)
                .HasForeignKey(e => e.MidResina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Reloj)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Reportes)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LotesAndOrdene>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<LotesAndOrdene>()
                .Property(e => e.LoteNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Reporte>()
                .Property(e => e.MId)
                .IsUnicode(false);

            modelBuilder.Entity<Reporte>()
                .Property(e => e.HU)
                .IsUnicode(false);

            modelBuilder.Entity<Reporte>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Reporte>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<Reporte>()
                .Property(e => e.UserName)
                .IsUnicode(false);
        }
    }
}
