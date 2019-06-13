using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 权限功能关系数据层
    /// </summary>
    public sealed class SingDevelopPowerFunService : BaseService, ISingDevelopPowerFun
    {
        #region 单例模式

        private static SingDevelopPowerFunService _singDevelopPowerFunService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingDevelopPowerFunService Instance
        {
            get
            {
                if (_singDevelopPowerFunService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singDevelopPowerFunService == null)
                        {
                            _singDevelopPowerFunService = new SingDevelopPowerFunService();
                        }
                    }
                }
                return _singDevelopPowerFunService;
            }
        }

        #endregion 单例模式

        #region Public Methods

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        public IList<DevelopPowerFun> GetDevelopPowerFunsByUserId(int userId)
        {
            return ExecuteReturnEntitysMethod<DevelopPowerFun>((_db) =>
            {
                return _db.DevelopPowerFuns.Include(i => i.DevelopFun).Where(w => w.UserId == userId).Include(d => d.DevelopFun).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 获取用户权限视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        public IList<View_DevelopUserPowerFun> GetViewDevelopUserPowerFunByUserId(int userId)
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.Database.SqlQuery<View_DevelopUserPowerFun>($"select * from View_DevelopUserPowerFun where UserId={userId} order by FunId asc").ToListAsync().Result;
            });
        }

        /// <summary>
        /// 设置用权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="funId">功能Id</param>
        /// <param name="isEnabled">是否启用</param>
        public bool SetDevelopPowerFun(int userId, int funId, bool isEnabled)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Database.ExecuteSqlCommand($"exec proc_SaveUserFun {userId},{funId},{isEnabled}");
            });
        }

        #endregion Public Methods
    }
}