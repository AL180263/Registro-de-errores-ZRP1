namespace Test_Entities.produccion
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LotesOrdene
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string OrdenId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string LoteNumber { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime FechaInsercion { get; set; }
    }
}
