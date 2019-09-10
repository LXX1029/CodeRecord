using DataEntitys;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Unity
{
    public interface IUnityUserFacade : IRepository<DevelopUser>
    {
        #region Public Methods
        Task<DevelopUser> GetDevelopUser(string name, string pwd);
        Task<IList<DevelopUser>> GetDevelopUsers(Expression<Func<DevelopUser, bool>> predicate = null);
        #endregion Public Methods
    }
}