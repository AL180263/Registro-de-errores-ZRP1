namespace Test_Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reportes
    {
        [Key]
        [Column(Order = 0)]
        public int Hash { get; set; }

        [StringLength(1)]
        public string MId { get; set; }

        [StringLength(1)]
        public string HU { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Turno { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Fecha { get; set; }

        [StringLength(1)]
        public string Descripcion { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Estatus { get; set; }

        [StringLength(128)]
        public string OrdenId { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(128)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Departamento { get; set; }

        public virtual Ordenes Ordenes { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}
