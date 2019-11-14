namespace Test_Entities.produccion
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class produccion : DbContext
    {
        public produccion()
            : base("name=produccion")
        {
        }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<LotesOrdene> LotesOrdenes { get; set; }
        public virtual DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<LotesOrdene>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<LotesOrdene>()
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
