using DataEntitys;
using Services.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Unity
{
    public interface IUnityDevelopPowerFunFacade : IRepository<DevelopPowerFun>
    {
        #region Public Methods

        Task<IList<DevelopPowerFun>> GetDevelopPowerFunsByUserId(int userId);
       
        Task<DevelopPowerFun> SetDevelopPowerFun(DevelopPowerFun developPowerFun);
        #endregion Public Methods
    }
}