using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Entities;

namespace Test_Entities
{
   public partial class BaseDeDatos:DbContext
    {
        public BaseDeDatos(): base("name=Prueba3")
        {

        }
        public virtual DbSet<Entities.Usuario> Usuarios { get; set; }

        public virtual DbSet<Entities.Lote> Lotes { get; set; }

        public virtual DbSet<Entities.LotesAndOrdenes> LotesAndOrdenes { get; set; }

        public virtual DbSet<Entities.Orden> Ordenes { get; set; }

        public virtual DbSet<Entities.Reporte> Reportes { get; set; }

        public virtual DbSet<Entities.Resina> Resinas { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Usuario>()
                .HasKey(o => o.UserName)
                .HasMany(e => e.Reportes)
                .WithOptional(e => e.Usuario)
                .HasForeignKey(e => e.UserName);
            modelBuilder.Entity<Entities.Lote>()
                .HasKey(k => k.LoteNumber);
                //.HasMany(o => o.Ordenes)
                //.WithRequired(e => e.Lote)
                //.HasForeignKey(f => f.LoteNumber);
            modelBuilder.Entity<Entities.Orden>().ToTable("Ordenes")
                .HasKey(k => k.OrdenId);
                //.HasMany(o => o.LotesAndOrdenes)
                //.WithOptional(e => e.Orden)
                //.HasForeignKey(f => f.OrdenId);
            modelBuilder.Entity<Entities.Orden>()
               .HasMany(o => o.Reportes)
               .WithOptional(e => e.Orden)
               .HasForeignKey(k => k.OrderId);
            modelBuilder.Entity<Entities.Resina>()
                .HasKey(k => k.MId)
               .HasMany(o => o.Lotes)
               .WithOptional(e => e.Resina)
               .HasForeignKey(k => k.MidResina);
            modelBuilder.Entity<Entities.Resina>()
               .HasMany(o => o.Ordenes)
               .WithOptional(e => e.Resina)
               .HasForeignKey(k => k.ResinaMid);
            modelBuilder.Entity<Entities.LotesAndOrdenes>()
                .HasKey(k => new { k.OrdenId, k.LoteNumber })
                .HasRequired(o => o.Orden)
                .WithMany(p => p.LotesAndOrdenes)
                .HasForeignKey(F => F.OrdenId);

            modelBuilder.Entity<Entities.LotesAndOrdenes>()
                .HasRequired(o => o.Lote)
                .WithMany(o => o.Ordenes)
                .HasForeignKey(f => f.LoteNumber);

              
            modelBuilder.Entity<Entities.Reporte>().HasKey(k => k.Hash);
               






        }

    }
}
