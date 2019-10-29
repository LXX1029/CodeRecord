using System.Collections.Generic;
using System.Threading.Tasks;
using DataEntitys;
using Services.Repositories;
namespace Services.Unity
{
    /// <summary>
    /// 类型接口
    /// </summary>
    public interface IUnityDevelopTypeFacade : IRepository<DevelopType>
    {
        Task<IList<DevelopType>> GetDevelopTypeListByFilter(string name, int parentId);
        Task<IList<DevelopType>> GetDevelopTypesByParentId(int parentId);
        Task<DevelopType> GetDevelopTypeByParentId(int parentId);
        Task<int> GetMaxDevelopTypeId();
    }
}