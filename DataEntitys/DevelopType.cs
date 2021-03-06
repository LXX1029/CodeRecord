﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntitys
{
    [Table("DevelopType")]
    public class DevelopType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int ParentId { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public virtual ICollection<DevelopRecord> DevelopRecords { get; set; }
    }
}