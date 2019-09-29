using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public partial class Resina
    {
        public Resina()
        {
            this.Lotes = new HashSet<Lote>();
            this.Ordenes = new HashSet<Orden>();
        }

        [Key]
        [Required]
        public string MId { get; set; }

        public int? GradosSecado { get; set; }

        public int? TiempoSecado { get; set; }

        public string Color { get; set; }

        public string Descripcion { get; set; }

        public virtual ICollection<Lote> Lotes { get; set; }
      
        public virtual ICollection<Orden> Ordenes { get; set; }
    }
}

