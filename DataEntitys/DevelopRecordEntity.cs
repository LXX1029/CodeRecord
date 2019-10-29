using System;
using System.Drawing;
using Common;
namespace DataEntitys
{
    /// <summary>
    /// 记录类
    /// </summary>
    public class DevelopRecordEntity
    {
        public DevelopRecordEntity()
        { }
        public DevelopRecordEntity(int userId) : this()
        {
            this.UserId = UserId;
        }
        private Bitmap _bitMap = null;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap BitMap
        {
            get
            {
                // 从数据库读取， 且Picture 必须有值
                return UtilityHelper.ConvertByteToImg(Picture);
            }
            set
            {
                _bitMap = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime { get; set; } = default(DateTime);

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; } = default(DateTime);

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 记录Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 父类型名称
        /// </summary>
        public string ParentTypeName { get; set; }

        /// <summary>
        /// 图片字节
        /// </summary>
        public byte[] Picture { get; set; } = null;
        /// <summary>
        /// 子 Id
        /// </summary>
        public int SubTypeId { get; set; }
        /// <summary>
        /// 子类型名称
        /// </summary>
        public string SubTypeName { get; set; }

        /// <summary>
        /// 添加记录的UserId
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 添加记录的用户名称
        /// </summary>
        public string UserName { get; set; }
    }
}