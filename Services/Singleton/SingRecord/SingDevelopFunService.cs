using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;
using System.Data.Entity;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 用户功能数据层
    /// </summary>
    public sealed class SingDevelopFunService : BaseService, ISingDevelopFun
    {
        #region 单例模式

        private static SingDevelopFunService _singDevelopFunService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingDevelopFunService Instance
        {
            get
            {
                if (_singDevelopFunService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singDevelopFunService == null)
                        {
                            _singDevelopFunService = new SingDevelopFunService();
                        }
                    }
                }
                return _singDevelopFunService;
            }
        }

        #endregion 单例模式

        #region Public Methods

        /// <summary>
        /// 获取用户功能权限
        /// </summary>
        /// <returns>IList</returns>
        public IList<DevelopFun> GetDevelopFuns()
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopFuns.ToListAsync().Result;
            });
        }

        #endregion Public Methods
    }
}