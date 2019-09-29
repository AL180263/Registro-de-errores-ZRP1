namespace Test_Entities.folder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lote()
        {
            LotesAndOrdenes = new HashSet<LotesAndOrdene>();
        }

        [Key]
        [StringLength(30)]
        public string LoteNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string MidResina { get; set; }

        public virtual Resina Resina { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotesAndOrdene> LotesAndOrdenes { get; set; }
    }
}
