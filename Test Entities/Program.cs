using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Test_Entities.folder;




namespace Test_Entities
{
    class Program
    {
        static void Main(string[] args)
        {
           
           

            Resinas resina = new Resinas() { MId = "705078-11", GradosSecado = 300, TiempoSecado = 4, Color = "Negro" };

            Ordenes orden = new Ordenes() { OrdenId = "6003590976", Resinas = resina };

            Lotes lote = new Lotes() { LoteNumber = "0011024943",MidResina = "705078-11"};

            Usuarios usuario = new Usuarios() { UserName = "MC1139", Nombre = "Melissa jazmin Carrera Ramos", Reloj = "236210", Acceso = 0};

            Reportes reporte = new Reportes() { UserName = "MC1139", Turno = 1, Estatus = 0, MId = "552210-000", Fecha = DateTime.Now, OrdenId = "6003590976", Departamento = 2 };


            try
            {

                using (Preliminar data = new Preliminar())
                {

                    //Lotes lote1 = new Lotes() { LoteNumber = "0011024949", MidResina = "705078-11" };
                    //Lotes lote2 = new Lotes() { LoteNumber = "0011024948", MidResina = "705078-11" };


                    //data.Resinas.Add(resina);
                    //data.Lotes.Add(lote1);
                    //data.Lotes.Add(lote2);
                    //data.Ordenes.Add(orden);

                    //data.Usuarios.Add(usuario);
                    //data.Reportes.Add(reporte);

                    //  data.SaveChanges();

                    var resultado = (from ret in data.Ordenes
                                    
                                    select ret).ToList();

                    var res = data.Reportes.FirstOrDefault();

                    var tr = res.Ordenes.LotesAndOrdenes.OrderByDescending(c => c.FechaInsercion).FirstOrDefault();

                    Console.WriteLine(tr.FechaInsercion);

                    foreach (var item in resultado)
                    {
                        Console.WriteLine("Orden: {0}, Resina: {1}", item.OrdenId, item.Resinas.MId);

                        foreach (var lotes in item.Resinas.Lotes)
                        {
                            Console.WriteLine("Lote:{0}", lotes.LoteNumber);
                        }
                    }

                    foreach (var item in data.Usuarios)
                    {
                        Console.WriteLine("Usuario {0}, Nombre : {1}", item.UserName, item.Nombre);
                    }

                    foreach (var item in data.Reportes)
                    {
                        Console.WriteLine("Numero de reporte {0}, Ordern {1}", item.Hash,item.OrdenId);

                    }
                }

                //using (CommscopeDb data = new CommscopeDb())
                //{
                //    var t = data.sysdiagrams;
                //    foreach (var item in t)
                //    {
                //        Console.WriteLine(item.name);
                //    }

                //}


                //using (Contexto dbcon = new Contexto())
                //{

                //    dbcon.Reportes.Add(reporte);

                //    dbcon.SaveChanges();
                //    Console.WriteLine("Reporte registrado");
                //    foreach (var item in dbcon.Usuarios)
                //    {
                //        Console.WriteLine(item);
                //    }
                  
                //}
            }
            catch (Exception Error)
            {
                Console.WriteLine(Error);
                
            }
            Console.ReadLine();
          
        }
       
      
    }

    //public class Contexto : DbContext
        
    //{
    //    public Contexto() : base("name=CommscopeDbEntities") { }
    //    public DbSet<Usuario> Usuarios { get; set; }

    //    public DbSet<Resina> Resinas { get; set; }

    //    public DbSet<Lote> Lotes { get; set; }

    //    public DbSet<Orden> Odenes { get; set; }

    //    public DbSet<Reporte> Reportes { get; set; }

    //    public DbSet<LotesAndOrdenes> LotesAndOrdenes { get; set; }


    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Usuario>().HasKey(o => o.UserName);
    //        modelBuilder.Entity<Resina>().HasKey(o => o.MId);
    //        modelBuilder.Entity<Orden>().HasKey(o => o.OrdenId);
    //        modelBuilder.Entity<Lote>().HasKey(o => o.LoteNumber);
    //        modelBuilder.Entity<Reporte>().HasKey(o => o.Hash);

    //        modelBuilder.Entity<LotesAndOrdenes>().HasKey(o => o.Id );


    //    }



    //}
}
