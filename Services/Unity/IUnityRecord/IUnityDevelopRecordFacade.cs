using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.Repositories;
namespace Services.Unity
{
    public interface IUnityDevelopRecordFacade : IRepository<DevelopRecord>
    {
        Task<bool> UpdateDevelopRecordClickCount(int recordId);
        Task<IList<DevelopRecordEntity>> GetDevelopRecordListByPager(int pageIndex, int pageCount);
        Task<int> GetDevelopRecordListCount();
        Task<int> GetMaxDevelopId();
    }
}
