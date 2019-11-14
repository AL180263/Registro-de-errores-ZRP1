using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Entities
{
    public enum Turno
    {
        A,
        B,
        C,
        D,
        Primero,
        Segundo,
        Tercero
    }

    public enum Estatus
    {
        Resuelto,
        Sin_Resolver
        
    }

    public enum Departamento
    {
        Produccion,
        Calidad,
        Planeacion,
        Almacen,
        Zrp1,

    }

    

    public partial class Reporte
    {

        public static readonly string[] Incidentes = new string[] { 
            "orden cerrada",
            "orden no se encuentra en ZRP1",
            "no hay suficiente cantidad en sistema",
            "error de costing",
            "incoherencia entre etiquetas (SAP,SAM)",
            "codigo de empaque no se encuentra",
            "material mal direccionado" };

        [Key]
        [Required]
        public int Hash { get; set; }
        public string Orden { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        public string Material { get; set; }

        public string Hu { get; set; }
        [Required]
        public string Incidente { get; set; }

        [Required]
        public Turno Turno { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public Departamento Departamento { get; set; }

        public string Descripcion { get; set; }
        [Required]
        public Estatus Estatus { get; set; }
        [Required]
        public string UserName { get; set; }

        

      




    }
}
