namespace Test_Entities.produccion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Reportes = new HashSet<Reporte>();
        }

        [Key]
        [StringLength(6)]
        public string UserName { get; set; }

        [Required]
        [StringLength(6)]
        public string Reloj { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public int Acceso { get; set; }

        public byte[] Imagen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reporte> Reportes { get; set; }
    }
}
