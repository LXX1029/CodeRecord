using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.CastleWindsor;

namespace Services.CastleWindsor
{
    public interface IDevelopRecordService : IRepository
    {
        Task<bool> UpdateDevelopRecordClickCount(int recordId);
        Task<IList<DevelopRecordEntity>> GetDevelopRecordListByPager(int pageIndex, int pageCount);
        Task<int> GetDevelopRecordListCount();
        Task<int> GetMaxDevelopId();
    }
}
