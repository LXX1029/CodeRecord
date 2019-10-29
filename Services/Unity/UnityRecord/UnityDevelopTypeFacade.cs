using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataEntitys;
using Services.EFCodeFirst;
using Services.Repositories;
using Services.Unity.UnityControl;

namespace Services.Unity
{
    /// <summary>
    /// 记录类型数据层
    /// </summary>
    public sealed class UnityDevelopTypeFacade : Repository<DevelopType>, IUnityDevelopTypeFacade
    {
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
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [UnityException]
        public async Task<IList<DevelopType>> GetDevelopTypeListByFilter(string name, int parentId)
        {
            return await this.GetEntities(m => m.Name == name && m.ParentId == parentId);
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        [UnityException]
        public async Task<IList<DevelopType>> GetDevelopTypesByParentId(int parentId)
        {
            return await this.GetEntities(m => m.ParentId == parentId);
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

        public override async Task<int> RemoveEntity(DevelopType t)
        {
            var affectedRows = 0;
            if (t == null) return affectedRows;
            using (var context = new RecordContext())
            using (var trans = context.Database.BeginTransaction())
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
                    affectedRows = await context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (System.Exception)
                {
                    trans.Rollback();
                }
            }
            return affectedRows;
        }
    }
}
