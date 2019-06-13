using Common;
using System;
using System.Drawing;

namespace Services.EF
{
    /// <summary>
    /// 记录类
    /// </summary>
    public class DevelopRecordEntity
    {
        public DevelopRecordEntity()
        {

        }

        public DevelopRecordEntity(int userId) : this()
        {
            this.UserId = UserId;
        }


        private Bitmap _bitMap = null;
        private int _childrenId = 0;
        private DateTime? _createdTime = new DateTime();
        private DateTime? _updatedTime = new DateTime();
        private string _desc = string.Empty;
        private int _id = 0;
        private int _idIndex = 0;
        private string _imagePath = string.Empty;

        private int _parentId = -1;

        private string _parentName = string.Empty;

        private byte[] _picture = null;

        private string _title = string.Empty;

        private string _typeName = string.Empty;

        private int _userId;

        private string _userName = "";

        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap BitMap
        {
            get
            {
                // 从数据库读取， 且Picture 必须有值
                return
                    UtilityHelper.ConvertByteToImg(Picture);
            }
            set
            {
                _bitMap = value;
            }
        }

        /// <summary>
        /// 子 Id
        /// </summary>
        public int ChildrenId
        {
            get { return _childrenId; }
            set { _childrenId = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdatedTime
        {
            get { return _updatedTime; }
            set { _updatedTime = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        /// <summary>
        /// 记录Id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Id 索引
        /// </summary>
        public int IdIndex
        {
            get { return _idIndex; }
            set { _idIndex = value; }
        }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        /// <summary>
        /// 父 Id
        /// </summary>
        public int ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        /// <summary>
        /// 父名称
        /// </summary>
        public string ParentName
        {
            get { return _parentName; }
            set { _parentName = value; }
        }

        /// <summary>
        /// 上次图片字节
        /// </summary>
        public byte[] Picture
        {
            get { return _picture; }
            set
            {
                _picture = value;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value.Trim(); }
        }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        /// <summary>
        /// 添加记录的UserId
        /// </summary>
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// 添加记录的用户名称
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// 是否为新对象
        /// </summary>
        public bool IsNew
        {
            get
            {
                return this.Id == 0;
            }
        }
    }
}