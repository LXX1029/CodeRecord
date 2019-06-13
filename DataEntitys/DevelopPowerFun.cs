using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntitys
{
    public class DevelopPowerFun
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual DevelopUser DevelopUser { get; set; }

        public int FunId { get; set; }
        [ForeignKey("FunId")]
        public virtual DevelopFun DevelopFun { get; set; }



    }
}