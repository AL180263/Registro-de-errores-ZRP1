namespace Test_Entities
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Entities;

    public class ModeloABaseDe : DbContext
    {
        public DbSet<Lote> Lotes { get; set; }

        public DbSet<Entities.LotesAndOrdenes> LotesAndOrdenes { get; set; }

        public DbSet<Orden> Ordenes { get; set; }

        public DbSet<Reporte> Reportes { get; set; }

        public DbSet<Resina> Resinas { get; set; }

        public DbSet<Entities.sysdiagrams> Sysdiagrams { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }



        // El contexto se ha configurado para usar una cadena de conexión 'ModeloABaseDe' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'Test_Entities.ModeloABaseDe' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'ModeloABaseDe'  en el archivo de configuración de la aplicación.
        public ModeloABaseDe()
            : base("name=Prueba1")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lote>().HasKey(o => o.LoteNumber);
            modelBuilder.Entity<Entities.LotesAndOrdenes>().HasKey(o => o.OrdenId);
            modelBuilder.Entity<Orden>().HasKey(o => o.OrdenId).ToTable("Ordenes");
            modelBuilder.Entity<Reporte>().HasKey(o => o.Hash);
            modelBuilder.Entity<Resina>().HasKey(o => o.MId);
            modelBuilder.Entity<Entities.sysdiagrams>().HasKey(o => o.principal_id);
            modelBuilder.Entity<Usuario>().HasKey(o => o.UserName);
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}