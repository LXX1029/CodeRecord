using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntitys
{
    [Table("DevelopRecord")]
    public class DevelopRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        [MaxLength(100), Required]
        public string Title { get; set; }

        public string Desc { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime UpdatedTime { get; set; } = DateTime.Now;
        public byte[] Picture { get; set; }
        public byte[] Zip { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual DevelopUser DevelopUser { get; set; }

        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual DevelopType DevelopType { get; set; }
    }
}