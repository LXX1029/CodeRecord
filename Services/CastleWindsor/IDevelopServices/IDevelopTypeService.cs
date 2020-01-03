using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
namespace Services.CastleWindsor
{
    /// <summary>
    /// 类型接口
    /// </summary>
    public interface IDevelopTypeService : IRepository
    {
        Task<IList<DevelopType>> GetDevelopTypeListByFilter(string name, int parentId);
        Task<IList<DevelopType>> GetDevelopTypesByParentId(int parentId);
        Task<DevelopType> GetDevelopTypeByParentId(int parentId);
        Task<int> GetMaxDevelopTypeId();
    }
}