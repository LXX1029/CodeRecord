using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.CastleWindsor;

namespace Services.CastleWindsor
{
    public interface IDevelopPowerFunService : IRepository
    {
        Task<IList<DevelopPowerFun>> GetDevelopPowerFunsByUserId(int userId);

        Task<DevelopPowerFun> SetDevelopPowerFun(DevelopPowerFun developPowerFun);
    }
}