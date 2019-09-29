namespace Test_Entities.folder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ordene
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordene()
        {
            LotesAndOrdenes = new HashSet<LotesAndOrdene>();
            Reportes = new HashSet<Reporte>();
        }

        [Key]
        [StringLength(30)]
        public string OrdenId { get; set; }

        [StringLength(30)]
        public string ResinaMid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LotesAndOrdene> LotesAndOrdenes { get; set; }

        public virtual Resina Resina { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reporte> Reportes { get; set; }
    }
}
