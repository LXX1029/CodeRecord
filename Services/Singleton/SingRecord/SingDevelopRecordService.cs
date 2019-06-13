using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;
using System.Data.Entity;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 记录项数据层
    /// </summary>
    public sealed class SingDevelopRecordService : BaseService, ISingDevelopRecord
    {

        #region 单例模式

        private static SingDevelopRecordService _singDevelopRecordService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingDevelopRecordService Instance
        {
            get
            {
                if (_singDevelopRecordService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singDevelopRecordService == null)
                        {
                            _singDevelopRecordService = new SingDevelopRecordService();
                        }
                    }
                }
                return _singDevelopRecordService;
            }
        }

        #endregion 单例模式

        #region 记录操作

        /// <summary>
        /// 添加记录
        /// </summary>
        public bool AddDevelopRecord(DevelopRecord record)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.DevelopRecords.Add(record);
            });
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="record">记录对象</param>
        public bool DeleteDevelopRecord(DevelopRecord record)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(record).State = EntityState.Deleted;
                _db.DevelopRecords.Remove(record);
            });
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="record">记录对象</param>
        public bool DeleteDevelopRecordById(int recordId)
        {
            DevelopRecord record = GetDevelopRecordById(recordId);
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(record).State = EntityState.Deleted;
                _db.DevelopRecords.Remove(record);
            });
        }

        /// <summary>
        /// 根据id 获取记录
        /// </summary>
        /// <param name="id">记录Id</param>
        public DevelopRecord GetDevelopRecordById(int id)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopRecords.SingleOrDefaultAsync(s => s.RecordId == id).Result;
            });
        }

        /// <summary>
        /// 批量 加载数据
        /// </summary>
        /// <param name="pageIndex">页</param>
        /// <param name="pageCount">每页数量</param>
        public IList<DevelopRecordEntity> GetDevelopRecordListByPager(int pageIndex, int pageCount)
        {
            return ExecuteSqlQueryReturnObjects<DevelopRecordEntity>($"exec proc_GetDevelopRecordPager { pageIndex},{ pageCount}");
        }

        /// <summary>
        /// 获取总记录数量
        /// </summary>
        /// <returns></returns>
        public int GetDevelopRecordListCount()
        {
            return ExecuteSqlQueryReturnObject<int>("select count(id) from DevelopRecord");
        }

        /// <summary>
        /// 更新developrecord
        /// </summary>
        public bool UpdateDevelopRecord(DevelopRecord record)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(record).State = EntityState.Modified;
            });
        }

        /// <summary>
        /// 更新记录点击次数
        /// </summary>
        /// <param name="recordId"></param>
        public bool UpdateDevelopRecordClickCount(int recordId)
        {
            return ExecuteSqlCommandReturnObject($"update DevelopRecord set ClickCount+=1 where Id={recordId}");
        }

        /// <summary>
        /// 获取最大Develop Id 值
        /// </summary>
        public int GetMaxDevelopId()
        {
            return ExecuteSqlQueryReturnObject<int>("select max(Id) from DevelopRecord");
        }

        #endregion 记录操作

    }
}
