namespace Test_Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lotes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lotes()
        {
            LotesAndOrdenes = new HashSet<LotesAndOrdenes>();
        }

        [Key]
        [StringLength(30)]
        public string LoteNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string MidResina { get; set; }

        public virtual Resinas Resinas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotesAndOrdenes> LotesAndOrdenes { get; set; }
    }
}
