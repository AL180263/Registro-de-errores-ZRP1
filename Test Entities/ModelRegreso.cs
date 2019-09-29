namespace Test_Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    //public partial class ModelRegreso : DbContext
    //{
    //    public ModelRegreso()
    //        : base("name=ModelRegreso")
    //    {
    //    }

    //    public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
    //    public virtual DbSet<Lote> Lotes { get; set; }
    //    public virtual DbSet<LotesAndOrdene> LotesAndOrdenes { get; set; }
    //    public virtual DbSet<Ordene> Ordenes { get; set; }
    //    public virtual DbSet<Reporte> Reportes { get; set; }
    //    public virtual DbSet<Resina> Resinas { get; set; }
    //    public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    //    public virtual DbSet<Usuario> Usuarios { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Lote>()
    //            .HasMany(e => e.LotesAndOrdenes)
    //            .WithOptional(e => e.Lote)
    //            .HasForeignKey(e => e.Lote_LoteNumber);

    //        modelBuilder.Entity<Ordene>()
    //            .HasMany(e => e.LotesAndOrdenes)
    //            .WithOptional(e => e.Ordene)
    //            .HasForeignKey(e => e.Orden_OrdenId);

    //        modelBuilder.Entity<Ordene>()
    //            .HasMany(e => e.Reportes)
    //            .WithOptional(e => e.Ordene)
    //            .HasForeignKey(e => e.Orden_OrdenId);

    //        modelBuilder.Entity<Resina>()
    //            .HasMany(e => e.Lotes)
    //            .WithOptional(e => e.Resina)
    //            .HasForeignKey(e => e.Resina_MId);

    //        modelBuilder.Entity<Resina>()
    //            .HasMany(e => e.Ordenes)
    //            .WithOptional(e => e.Resina)
    //            .HasForeignKey(e => e.Resina_MId);

    //        modelBuilder.Entity<Usuario>()
    //            .HasMany(e => e.Reportes)
    //            .WithOptional(e => e.Usuario)
    //            .HasForeignKey(e => e.Usuario_UserName);
    //    }
    //}
}
