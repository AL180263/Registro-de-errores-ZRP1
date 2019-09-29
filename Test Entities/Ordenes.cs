namespace Test_Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ordenes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordenes()
        {
            LotesAndOrdenes = new HashSet<LotesAndOrdenes>();
            Reportes = new HashSet<Reportes>();
        }

        [Key]
        [StringLength(10)]
        public string OrdenId { get; set; }

        [StringLength(30)]
        public string ResinaMid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotesAndOrdenes> LotesAndOrdenes { get; set; }

        public virtual Resinas Resinas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reportes> Reportes { get; set; }
    }
}
