using DataEntitys;
using Services.Unity.UnityControl;
using System;
using System.Data.Entity;
using System.Linq;
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
            return await DbContext.DevelopUsers.Include("DevelopPowerFuns.DevelopFun").SingleOrDefaultAsync(s => s.Name == name && s.Pwd == pwd);
        }

        /// <summary>
        /// 加载所有用户
        /// </summary>
        [UnityException]
        public IQueryable<DevelopUser> GetDevelopUsers()
        {
            return DbContext.DevelopUsers.Include(m => m.DevelopPowerFuns);
        }
        #endregion 用户管理
    }
}
