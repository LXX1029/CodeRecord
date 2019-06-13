using DataEntitys;
using Services.Repositories;
using Services.Unity.UnityControl;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace Services.Unity
{
    /// <summary>
    /// 统计模块数据层
    /// </summary>
    public sealed class UnityStatisticsFacade : Repository<ClickCountReportEntity>, IUnityStatisticsFacade
    {
        #region Public Methods
        [UnityException]
        public IQueryable<ClickCountReportEntity> GetClickCountReport()
        {
            var reportData = from p in DbContext.DevelopRecords
                             group p by new { p.TypeId, p.DevelopType } into g
                             let parentType = DbContext.DevelopTypes.FirstOrDefault(s => s.Id == g.Key.DevelopType.ParentId)
                             select new ClickCountReportEntity
                             {
                                 ParentTypeId = parentType.Id,
                                 ParentTypeName = parentType.Name,
                                 SubTypeId = g.Key.DevelopType.Id,
                                 SubTypeName = g.Key.DevelopType.Name,
                                 ClickCount = g.Sum(s => s.ClickCount)
                             };
            return reportData;
        }

        #endregion Public Methods
    }
}