using DataEntitys;
using Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Unity
{
    public interface IUnityDevelopRecordFacade : IRepository<DevelopRecord>
    {
        #region 记录操作
        Task<bool> UpdateDevelopRecordClickCount(int recordId);
        Task<IList<DevelopRecordEntity>> GetDevelopRecordListByPager(int pageIndex, int pageCount);
        Task<int> GetDevelopRecordListCount();
        Task<int> GetMaxDevelopId();
        #endregion 记录操作

    }
}
