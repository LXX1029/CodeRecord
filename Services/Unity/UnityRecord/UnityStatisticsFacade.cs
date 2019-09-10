using DataEntitys;
using Services.Repositories;
using Services.Unity.UnityControl;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Services.EFCodeFirst;
using System.Data.Entity;

namespace Services.Unity
{
    /// <summary>
    /// 统计模块数据层
    /// </summary>
    public sealed class UnityStatisticsFacade : Repository<ClickCountReportEntity>, IUnityStatisticsFacade
    {
        #region Public Methods
        [UnityException]
        public async Task<List<ClickCountReportEntity>> GetClickCountReport()
        {
            using (var context = new RecordContext())
            {
                return await (from p in context.DevelopRecords
                              group p by new { p.TypeId, p.DevelopType } into g
                              let parentType = context.DevelopTypes.FirstOrDefault(s => s.Id == g.Key.DevelopType.ParentId)
                              select new ClickCountReportEntity
                              {
                                  ParentTypeId = parentType.Id,
                                  ParentTypeName = parentType.Name,
                                  SubTypeId = g.Key.DevelopType.Id,
                                  SubTypeName = g.Key.DevelopType.Name,
                                  ClickCount = g.Sum(s => s.ClickCount)
                              }).ToListAsync();
            }
        }

        #endregion Public Methods
    }
}