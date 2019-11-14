using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
  public partial class LoteOrden
    {
        

        
        [Key]
        
        public string Orden { get; set; }
        [Key]
        public string Lote { get; set; }

       

        //public Orden Ordenes { get; set; }
        //public Lote Lotes { get; set; }
        public DateTime FechaInsercion { get; set; } 

       

    }
}
