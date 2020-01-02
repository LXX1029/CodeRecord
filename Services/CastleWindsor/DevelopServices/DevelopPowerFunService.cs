using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataEntitys;
using Services.EFCodeFirst;
using Services.Repositories;
namespace Services.CastleWindsor.DevelopServices
{
    /// <summary>
    /// 权限功能关系数据层
    /// </summary>
    public sealed class DevelopPowerFunService : Repository, IDevelopPowerFunService
    {
        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        public async Task<IList<DevelopPowerFun>> GetDevelopPowerFunsByUserId(int userId)
        {
            using (var context = new RecordContext())
                return await context.DevelopPowerFuns.Include(i => i.DevelopFun).Where(w => w.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="developPowerFun">DevelopPowerFun</param>
        public async Task<DevelopPowerFun> SetDevelopPowerFun(DevelopPowerFun developPowerFun)
        {
            DevelopPowerFun currentDevelopPowerFun = null;
            using (var context = new RecordContext())
            {
                currentDevelopPowerFun = context.DevelopPowerFuns.FirstOrDefault(m => m.UserId == developPowerFun.UserId && m.FunId == developPowerFun.FunId);

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
        }
    }
}