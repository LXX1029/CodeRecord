using DataEntitys;
using Services.Repositories;
using Services.Unity.UnityControl;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Unity
{
    /// <summary>
    /// 权限功能关系数据层
    /// </summary>
    public sealed class UnityDevelopPowerFunFacade : Repository<DevelopPowerFun>, IUnityDevelopPowerFunFacade
    {
        #region Public Methods

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        public async Task<IList<DevelopPowerFun>> GetDevelopPowerFunsByUserId(int userId)
        {
            return await DbContext.DevelopPowerFuns.Include(i => i.DevelopFun).Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<DevelopPowerFun> SetDevelopPowerFun(DevelopPowerFun developPowerFun)
        {
            DevelopPowerFun currentDevelopPowerFun = DbContext.DevelopPowerFuns.FirstOrDefault(m => m.UserId == developPowerFun.UserId && m.FunId == developPowerFun.FunId);
            if (currentDevelopPowerFun != null)
            {
                currentDevelopPowerFun.IsEnabled = developPowerFun.IsEnabled;
                return await UpdateEntity(currentDevelopPowerFun);
            }
            else
            {
                return await AddEntity(developPowerFun);
            }
        }

        #endregion Public Methods
    }
}