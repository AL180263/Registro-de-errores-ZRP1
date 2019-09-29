namespace Test_Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Preliminar : DbContext
    {
        public Preliminar()
            : base("name=Preliminar")
        {
        }

        public virtual DbSet<Lotes> Lotes { get; set; }
        public virtual DbSet<Ordenes> Ordenes { get; set; }
        public virtual DbSet<Resinas> Resinas { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<LotesAndOrdenes> LotesAndOrdenes { get; set; }
        public virtual DbSet<Reportes> Reportes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lotes>()
                .Property(e => e.LoteNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Lotes>()
                .Property(e => e.MidResina)
                .IsUnicode(false);

            modelBuilder.Entity<Ordenes>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<Ordenes>()
                .Property(e => e.ResinaMid)
                .IsUnicode(false);

            modelBuilder.Entity<Resinas>()
                .Property(e => e.MId)
                .IsUnicode(false);

            modelBuilder.Entity<Resinas>()
                .Property(e => e.Color)
                .IsUnicode(false);

            modelBuilder.Entity<Resinas>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Resinas>()
                .HasMany(e => e.Lotes)
                .WithRequired(e => e.Resinas)
                .HasForeignKey(e => e.MidResina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resinas>()
                .HasMany(e => e.Ordenes)
                .WithOptional(e => e.Resinas)
                .HasForeignKey(e => e.ResinaMid);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Reloj)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Reportes)
                .WithRequired(e => e.Usuarios)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LotesAndOrdenes>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<LotesAndOrdenes>()
                .Property(e => e.LoteNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Reportes>()
                .Property(e => e.MId)
                .IsUnicode(false);

            modelBuilder.Entity<Reportes>()
                .Property(e => e.HU)
                .IsUnicode(false);

            modelBuilder.Entity<Reportes>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Reportes>()
                .Property(e => e.OrdenId)
                .IsUnicode(false);

            modelBuilder.Entity<Reportes>()
                .Property(e => e.UserName)
                .IsUnicode(false);
        }
    }
}
