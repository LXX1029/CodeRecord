using DataEntitys;
using Services.Repositories;
//using Services.Singleton.ISingRecord;
using Services.Unity.UnityControl;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Services.EFCodeFirst;

namespace Services.Unity
{
    /// <summary>
    /// 记录项数据层
    /// </summary>
    public sealed class UnityDevelopRecordFacade : Repository<DevelopRecord>, IUnityDevelopRecordFacade
    {
        #region 记录操作
        /// <summary>
        /// 批量 加载数据
        /// </summary>
        /// <param name="pageIndex">页</param>
        /// <param name="pageCount">每页数量</param>
        public async Task<IList<DevelopRecordEntity>> GetDevelopRecordListByPager(int pageIndex = 0, int pageCount = 10)
        {
            if (pageIndex < 0) pageIndex = 0;
            if (pageCount < 0) pageCount = 10;
            using (var context = new RecordContext())
            {
                return await (from p in context.DevelopRecords.Include("DevelopType")
                              select new DevelopRecordEntity
                              {
                                  Id = p.Id,
                                  ParentId = p.DevelopType.ParentId,
                                  ParentTypeName = context.DevelopTypes.FirstOrDefault(m => m.Id == p.DevelopType.ParentId).Name,
                                  SubTypeId = p.DevelopType.Id,
                                  SubTypeName = p.DevelopType.Name,
                                  Title = p.Title,
                                  Desc = p.Desc,
                                  Picture = p.Picture,
                                  UserId = p.DevelopUser.Id,
                                  UserName = p.DevelopUser.Name,
                                  CreatedTime = p.CreatedTime,
                                  UpdatedTime = p.UpdatedTime

                              }).OrderByDescending(m => m.CreatedTime).Skip(pageIndex * pageCount).Take(pageCount).ToListAsync();
            }
        }

        /// <summary>
        /// 获取总记录数量
        /// </summary>
        public async Task<int> GetDevelopRecordListCount()
        {
            using (var context = new RecordContext())
                return await context.DevelopRecords.CountAsync();
        }

        /// <summary>
        /// 更新记录点击次数
        /// </summary>
        /// <param name="recordId">记录Id</param>
        [UnityException]
        public async Task<bool> UpdateDevelopRecordClickCount(int recordId)
        {
            var entity = await GetEntity(recordId);
            if (entity == null) return false;
            entity.ClickCount += 1;
            await UpdateEntity(entity);
            return true;
        }

        /// <summary>
        /// 获取最大Develop Id 值
        /// </summary>
        [UnityException]
        public async Task<int> GetMaxDevelopId()
        {
            using (var context = new RecordContext())
                return await context.DevelopRecords.MaxAsync(m => m.Id);
        }
        #endregion 记录操作

    }
}
