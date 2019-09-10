using DataEntitys;
using Services.EFCodeFirst;
using Services.Unity.UnityControl;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Unity
{
    /// <summary>
    /// 用户模块数据层
    /// </summary>
    public sealed class UnityUserFacade : Repositories.Repository<DevelopUser>, IUnityUserFacade
    {
        #region 用户管理
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="pwd">密码</param>
        [UnityException]
        public async Task<DevelopUser> GetDevelopUser(string name, string pwd)
        {
            using (var context = new RecordContext())
                return await context.DevelopUsers.Include("DevelopPowerFuns.DevelopFun").SingleOrDefaultAsync(s => s.Name == name && s.Pwd == pwd);
        }

        /// <summary>
        /// 加载所有用户
        /// </summary>
        [UnityException]
        public async Task<IList<DevelopUser>> GetDevelopUsers(Expression<Func<DevelopUser, bool>> predicate = null)
        {
            using (var context = new RecordContext())
            {
                if (predicate == null)
                    return await context.DevelopUsers.Include(m => m.DevelopPowerFuns).ToListAsync();
                else
                    return await context.DevelopUsers.Include(m => m.DevelopPowerFuns).Where(predicate).ToListAsync();
            }
        }
        #endregion 用户管理
    }
}
