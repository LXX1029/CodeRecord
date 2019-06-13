using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.EF
{
    sealed class CodeRecordEFService : BaseService
    {
        #region 单例模式

        private static CodeRecordEFService _codeRecordEFService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static CodeRecordEFService Instance
        {
            get
            {
                if (_codeRecordEFService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_codeRecordEFService == null)
                        {
                            _codeRecordEFService = new CodeRecordEFService();
                        }
                    }
                }
                return _codeRecordEFService;
            }
        }

        #endregion 单例模式

        #region 用户管理

        public bool AddUser(DevelopUser user)
        {
            //using (var cxt = new RecordDBEntities())
            //{
            //    cxt.DevelopUsers.Add(user);
            //    /*第二种形式 非同一上下文的对象要设置其状态为Add*/
            //    //cxt.Entry(user).State = System.Data.Entity.EntityState.Added;
            //    return cxt.SaveChanges() > 0 ? true : false;
            //}
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.DevelopUsers.Add(user);
                /*第二种形式 非同一上下文的对象要设置其状态为Add*/
                //cxt.Entry(user).State = System.Data.Entity.EntityState.Added;
            });
        }

        public bool DeleteUser(DevelopUser user)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(user).State = EntityState.Deleted;
                _db.DevelopUsers.Remove(user);
            });
        }

        public bool DeleteUserByUserId(int userId)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                DevelopUser user = _db.DevelopUsers.SingleOrDefault(s => s.UserId == userId);
                _db.DevelopUsers.Remove(user);
            });
        }
        public bool UpdateUser(DevelopUser user)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                // 非同一上下文的对象要设置其状态
                _db.Entry(user).State = EntityState.Modified;
            });
        }
        public DevelopUser GetDevelopUser(string name, string pwd)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopUsers.Include("DevelopPowerFuns.DevelopFun").SingleOrDefaultAsync(s => s.Name == name && s.Pwd == pwd).Result;
            });
        }

        public DevelopUser GetDevelopUserByUserId(int userId)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopUsers.SingleOrDefaultAsync(s => s.UserId == userId).Result;
            });
        }

        public DevelopUser GetDevelopUserByUserIdAsync(int userId)
        {
            using (var cxt = new RecordDBEntities())
            {
                return cxt.DevelopUsers.SingleOrDefaultAsync(s => s.UserId == userId).Result;
            }
        }

        /// <summary>
        /// 加载所有用户
        /// </summary>
        public IList<DevelopUser> GetDevelopUsers()
        {
            var result = ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopUsers.Include(u => u.DevelopPowerFuns).ToListAsync().Result;
            });
            return result;
        }
        #endregion 用户管理

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
        /// <returns></returns>
        public bool UpdateDevelopRecordClickCount(int recordId)
        {
            return ExecuteSqlCommandReturnObject($"update DevelopRecord set ClickCount+=1 where Id={recordId}");
        }

        #endregion 记录操作

        #region 记录类型操作

        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type">要添加的类型</param>
        public bool AddDevelopType(DevelopType type)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.DevelopTypes.Add(type);
            });
        }

        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="typeId">类型Id</param>
        public bool DeleteDeveloptype(DevelopType type)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                // 获取删除子版块
                if (type.ParentId == 0)
                {
                    foreach (DevelopType subType in _db.DevelopTypes.Where(w => w.ParentId == type.TypeId))
                    {
                        _db.Entry(subType).State = EntityState.Deleted;
                        //_db.DevelopTypes.Remove(type);
                    }
                }
                _db.Entry(type).State = EntityState.Deleted;
                _db.DevelopTypes.Remove(type);
            });
        }

        /// <summary>
        /// 根据Id 获取类型对象
        /// </summary>
        /// <param name="id">Id</param>
        public DevelopType GetDevelopTypeById(int id)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopTypes.SingleOrDefault(s => s.TypeId == id);
            });
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        public IList<DevelopType> GetDevelopTypeList()
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopTypes.OrderByDescending(o => o.UpdatedTime).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 查询类型
        /// </summary>
        /// <param name="name">类型名称</param>
        /// <param name="parentId">父Id</param>
        public IList<DevelopType> GetDevelopTypeListByFilter(string name, int parentId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ExecuteReturnEntitysMethod((_db) =>
               {
                   return _db.DevelopTypes.ToListAsync().Result;
               });
            }
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopTypes.Where(w => w.Name == name && w.ParentId == parentId).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        public IList<DevelopType> GetDevelopTypesByParentId(int parentId)
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopTypes.Where(w => w.ParentId == parentId).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 获取最大类型Id
        /// </summary>
        public int GetMaxDevelopTypeId()
        {
            return ExecuteReturnObjectMethod((_db) =>
            {
                return _db.DevelopTypes.Max(m => m.TypeId);
            });
        }

        /// <summary>
        /// 更新类型
        /// </summary>
        /// <param name="type">原类型对象</param>
        public bool UpdateDeveloptype(DevelopType type)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(type).State = EntityState.Modified;
            });
        }

        #endregion 记录类型操作

        #region 功能项操作

        public IList<DevelopFun> GetDevelopFuns()
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopFuns.ToListAsync().Result;
            });
        }

        #endregion 功能项操作

        #region 权限功能操作

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        public IList<DevelopPowerFun> GetDevelopPowerFunsByUserId(int userId)
        {
            return ExecuteReturnEntitysMethod<DevelopPowerFun>((_db) =>
            {
                return _db.DevelopPowerFuns.Include(i => i.DevelopFun).Where(w => w.UserId == userId).Include(d => d.DevelopFun).ToListAsync().Result;
            });
        }

        /// <summary>
        /// 获取用户权限视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        public IList<View_DevelopUserPowerFun> GetViewDevelopUserPowerFunByUserId(int userId)
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.Database.SqlQuery<View_DevelopUserPowerFun>($"select * from View_DevelopUserPowerFun where UserId={userId} order by FunId asc").ToListAsync().Result;
            });
        }

        /// <summary>
        /// 设置用权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="funId">功能Id</param>
        /// <param name="isEnabled">是否启用</param>
        public bool SetDevelopPowerFun(int userId, int funId, bool isEnabled)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Database.ExecuteSqlCommand($"exec proc_SaveUserFun {userId},{funId},{isEnabled}");
            });
        }

        #endregion 权限功能操作

        #region 统计

        public IList<ClickCountReportEntity> GetClickCountReport()
        {
            return ExecuteSqlQueryReturnObjects<ClickCountReportEntity>("exec proc_ClickCountReport");
        }

        #endregion 统计
    }
}