using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.Repositories;
namespace Services.Unity
{
    public interface IUnityStatisticsFacade : IRepository<ClickCountReportEntity>
    {
        Task<List<ClickCountReportEntity>> GetClickCountReport();
    }
}