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
        /// <inheritdoc/>
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
                                  ClickCount = g.Sum(s => s.ClickCount),
                              }).ToListAsync();
            }
        }
    }
}