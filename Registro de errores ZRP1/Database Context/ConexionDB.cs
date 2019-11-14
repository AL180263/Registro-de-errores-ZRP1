using System;
using System.Data.Entity;
using System.Linq;
using Entities;




namespace Registro_de_errores_ZRP1.Database_Context
{
    
   

    public class ConexionDB : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'ConexionDB' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'Registro_de_errores_ZRP1.Database_Context.ConexionDB' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'ConexionDB'  en el archivo de configuración de la aplicación.

        public virtual DbSet<Reporte> Reportes { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        public virtual DbSet<LoteOrden> LotesOrdenes { get; set; }


        public ConexionDB()
            : base("name=ConexionDB")
        {


        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().HasKey(k => k.UserName);
            modelBuilder.Entity<Reporte>().HasKey(k => k.Hash);
            modelBuilder.Entity<LoteOrden>().HasKey(k => new { k.Orden, k.Lote }).ToTable("LotesOrdenes");

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Reportes)
                .WithRequired(e => e.Usuario).HasForeignKey(f => f.UserName)
                .WillCascadeOnDelete(false);

                

        }

       
    }

}