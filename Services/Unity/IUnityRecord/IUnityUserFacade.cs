using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataEntitys;
using Services.Repositories;
namespace Services.Unity
{
    public interface IUnityUserFacade : IRepository<DevelopUser>
    {
        Task<DevelopUser> GetDevelopUser(string name, string pwd);

        Task<IList<DevelopUser>> GetDevelopUsers(Expression<Func<DevelopUser, bool>> predicate = null);
    }
}