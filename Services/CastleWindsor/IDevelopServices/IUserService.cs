using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataEntitys;
namespace Services.CastleWindsor
{
    public interface IUserService : IRepository
    {
        Task<DevelopUser> GetDevelopUser(string name, string pwd);

        Task<IList<DevelopUser>> GetDevelopUsers(Expression<Func<DevelopUser, bool>> predicate = null);
    }
}