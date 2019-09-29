using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;
namespace Entities
{
    public partial class Lote
    {

        public Lote()
        {
            this.Ordenes = new HashSet<LotesAndOrdenes>();
        }
        [Key]
        [Required]
        public string LoteNumber { get; set; }

        
        public string MidResina { get; set; }

        public virtual Resina  Resina { get; set; }
        
        [Required]
        public virtual ICollection<LotesAndOrdenes> Ordenes { get; set; }

    

    }
}
