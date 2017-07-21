namespace Kukil.Service2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PickingNumber")]
    public partial class PickingNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransId { get; set; }

        public int TransSeq { get; set; }

        public DateTime UpdatedAt { get; set; }

        [StringLength(150)]
        public string LastGenString { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
