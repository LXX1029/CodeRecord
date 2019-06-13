using Services.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Singleton.ISingRecord
{
    internal interface ISingDevelopRecord
    {

        #region 记录操作
        bool AddDevelopRecord(DevelopRecord record);
        bool DeleteDevelopRecord(DevelopRecord record);
        bool DeleteDevelopRecordById(int recordId);
        bool UpdateDevelopRecord(DevelopRecord record);
        bool UpdateDevelopRecordClickCount(int recordId);
        DevelopRecord GetDevelopRecordById(int id);
        IList<DevelopRecordEntity> GetDevelopRecordListByPager(int pageIndex, int pageCount);
        int GetDevelopRecordListCount();
        #endregion 记录操作

    }
}
