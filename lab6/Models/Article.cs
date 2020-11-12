namespace lab6.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Article")]
    public partial class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Tittle { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        [StringLength(50)]
        public string Author { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DatePulished { get; set; }

        public int? CateId { get; set; }

        public virtual Category Category { get; set; }
    }
}
