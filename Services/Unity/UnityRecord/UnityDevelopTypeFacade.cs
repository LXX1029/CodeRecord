using DataEntitys;
using Services.EFCodeFirst;
using Services.Repositories;
//using Services.Singleton.ISingRecord;
using Services.Unity.UnityControl;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Unity
{
    /// <summary>
    /// 记录类型数据层
    /// </summary>
    public sealed class UnityDevelopTypeFacade : Repository<DevelopType>, IUnityDevelopTypeFacade
    {

        #region 记录类型操作
        /// <summary>
        /// parentId 获取类型对象
        /// </summary>
        /// <param name="parentId">parentId</param>
        /// <returns>DevelopType</returns>
        [UnityException]
        public async Task<DevelopType> GetDevelopTypeByParentId(int parentId)
        {
            using (var context = new RecordContext())
                return await context.DevelopTypes.SingleOrDefaultAsync(s => s.Id == parentId);
        }

        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="name">类型名称</param>
        /// <param name="parentId">父Id</param>
        [UnityException]
        public async Task<IList<DevelopType>> GetDevelopTypeListByFilter(string name, int parentId)
        {
            return await GetEntities(m => m.Name == name && m.ParentId == parentId);
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        [UnityException]
        public async Task<IList<DevelopType>> GetDevelopTypesByParentId(int parentId)
        {
            return await GetEntities(m => m.ParentId == parentId);
        }

        /// <summary>
        /// 获取最大类型Id
        /// </summary>
        [UnityException]
        public async Task<int> GetMaxDevelopTypeId()
        {
            using (var context = new RecordContext())
                return await context.DevelopTypes.MaxAsync(m => m.Id);
        }

        public override async Task<bool> RemoveEntity(DevelopType t)
        {
            if (t == null) return false;
            using (var context = new RecordContext())
            using (DbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    var removeRecordList = context.DevelopRecords.Where(x => x.TypeId == t.Id);
                    if (removeRecordList.Count() > 0)
                    {
                        foreach (var item in removeRecordList)
                            context.Entry<DevelopRecord>(item).State = EntityState.Deleted;
                    }
                    context.Entry<DevelopType>(t).State = EntityState.Deleted;
                    await context.SaveChangesAsync();
                    trans.Commit();
                    return true;
                }
                catch (System.Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
        #endregion 记录类型操作
    }
}
