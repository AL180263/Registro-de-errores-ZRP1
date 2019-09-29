using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
  public partial class LotesAndOrdenes
    {
       
        [Key]
        
        public string OrdenId { get; set; }
        [Key]
        public string LoteNumber { get; set; }

        public virtual Orden Orden { get; set; }
        public virtual Lote Lote { get; set; }

        //public Orden Ordenes { get; set; }
        //public Lote Lotes { get; set; }
        public DateTime FechaInsercion { get; set; } = DateTime.Now;

       

    }
}
