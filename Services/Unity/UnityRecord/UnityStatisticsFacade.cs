namespace Services.Unity
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using DataEntitys;
    using Services.EFCodeFirst;
    using Services.Repositories;
    using Services.Unity.UnityControl;

    /// <summary>
    /// 统计模块数据层
    /// </summary>
    public sealed class UnityStatisticsFacade : Repository<ClickCountReportEntity>, IUnityStatisticsFacade
    {
        public async Task<List<ClickCountReportEntity>> GetClickCountReport()
        {
            using (var context = new RecordContext())
            {
                return await (from p in context.DevelopRecords.Include(m => m.DevelopType)
                              join q in context.DevelopTypes on p.DevelopType.ParentId equals q.Id
                              group p by new { q.Name, p.DevelopType } into g
                              select new ClickCountReportEntity
                              {
                                  ParentTypeId = g.Key.DevelopType.ParentId,
                                  ParentTypeName = g.Key.Name,
                                  SubTypeId = g.Key.DevelopType.Id,
                                  SubTypeName = g.Key.DevelopType.Name,
                                  ClickCount = g.Sum(s => s.ClickCount),
                              }).ToListAsync();

            }
        }
    }
}