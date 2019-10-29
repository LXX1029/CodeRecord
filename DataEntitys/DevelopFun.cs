using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntitys
{
    [Table("DevelopFun")]
    public class DevelopFun
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 图标对应索引
        /// </summary>
        public int ImageIndex { get; set; } = -1;

        /// <summary>
        /// 父功能Id
        /// </summary>
        [Required]
        public int ParentID { get; set; }
    }
}