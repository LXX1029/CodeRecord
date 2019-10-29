using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.Repositories;
namespace Services.Unity
{
    public interface IUnityDevelopPowerFunFacade : IRepository<DevelopPowerFun>
    {
        Task<IList<DevelopPowerFun>> GetDevelopPowerFunsByUserId(int userId);

        Task<DevelopPowerFun> SetDevelopPowerFun(DevelopPowerFun developPowerFun);
    }
}