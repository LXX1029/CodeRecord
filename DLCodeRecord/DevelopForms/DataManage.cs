using System.Collections.Generic;
using System.ComponentModel;
using DataEntitys;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 数据管理类
    /// </summary>
    public sealed class DataManage
    {
        #region 字段
        /// <summary>
        /// 默认登陆用户
        /// </summary>
        public static DevelopUser LoginUser = new DevelopUser();
        #endregion 字段

        #region 单例模式
        private static readonly object Obj = new object();
        private DataManage()
        { }
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns>DataManage</returns>
        private static DataManage dataManage = null;
        public static DataManage Instance
        {
            get
            {
                if (dataManage == null)
                {
                    lock (Obj)
                    {
                        if (dataManage == null)
                            dataManage = new DataManage();
                    }
                }
                return dataManage;
            }
        }
        #endregion

        #region 数据集合
        /// <summary>
        /// 功能集合
        /// </summary>
        public BindingList<DevelopFun> DevelopFunList { get; set; } = new BindingList<DevelopFun>();

        /// <summary>
        /// 权限功能集合
        /// </summary>
        public List<DevelopPowerFun> DevelopPowerFunList { get; set; } = new List<DevelopPowerFun>();

        /// <summary>
        /// 记录数据集合
        /// </summary>
        public BindingList<DevelopRecordEntity> DevelopRecordEntityList { get; set; } = new BindingList<DevelopRecordEntity>();

        /// <summary>
        /// 用户集合
        /// </summary>
        public BindingList<DevelopUser> DevelopUserList { get; set; } = new BindingList<DevelopUser>();

        #endregion

        // 由GC调用
        /*~DataManage()
        {
            DevelopUserList?.Clear();
            DevelopRecordEntityList?.Clear();
            DevelopPowerFunList?.Clear();
            DevelopFunList?.Clear();
            LoggerHelper.WriteOperation("已清理所有数据集合");
        }*/
    }
}