using DataEntitys;
using Services.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Unity
{
    public interface IUnityUserFacade : IRepository<DevelopUser>
    {
        #region Public Methods
        Task<DevelopUser> GetDevelopUser(string name, string pwd);
        IQueryable<DevelopUser> GetDevelopUsers();
        #endregion Public Methods
    }
}