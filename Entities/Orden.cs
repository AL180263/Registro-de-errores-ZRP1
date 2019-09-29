using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
   public partial class Orden
    {
        public Orden()
        {
            this.LotesAndOrdenes = new HashSet<LotesAndOrdenes>();
            this.Reportes = new HashSet<Reporte>();
        }

        [Key,StringLength(10)]
        public string OrdenId { get; set; }

        public string ResinaMid { get; set; }

        public virtual Resina Resina { get; set; }

        public virtual ICollection<LotesAndOrdenes> LotesAndOrdenes { get; set; }
        
        public virtual ICollection<Reporte> Reportes { get; set; }


    }
}
