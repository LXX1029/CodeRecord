using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
namespace Services.CastleWindsor
{
    public interface IStatisticsService : IRepository
    {
        Task<List<ClickCountReportEntity>> GetClickCountReport();
    }
}