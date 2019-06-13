﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntitys
{
    public class DevelopUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(15), Required]
        public string Name { get; set; }
        [MaxLength(50), Required]
        public string Pwd { get; set; }
        [MaxLength(2)]
        public string Sex { get; set; }

        public decimal DevelopAge { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<DevelopRecord> DevelopRecords { get; set; }

        public virtual ICollection<DevelopPowerFun> DevelopPowerFuns { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }
    }
}