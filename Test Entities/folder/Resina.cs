namespace Test_Entities.folder
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Resina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resina()
        {
            Lotes = new HashSet<Lote>();
            Ordenes = new HashSet<Ordene>();
        }

        [Key]
        [StringLength(30)]
        public string MId { get; set; }

        public int? GradosSecado { get; set; }

        public int? TiempoSecado { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(50)]
        public string Descripcion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lote> Lotes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordene> Ordenes { get; set; }
    }
}
