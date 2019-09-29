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
        Resolviendo,
        Sin_Resolver
        
    }

    public partial class Reporte
    {

        [Key]
        [Required]
        public int Hash { get; set; }
        public virtual Orden Orden { get; set; }

        [Required]
        public virtual Usuario Usuario { get; set; }

        public string Material { get; set; }

        public string Hu { get; set; }

        public Turno Turno { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string Descripcion { get; set; }

        public Estatus Estatus { get; set; }
        [Required]
        public string UserName { get; set; }

        public String OrderId { get; set; }

      




    }
}
