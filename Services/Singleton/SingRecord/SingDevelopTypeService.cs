using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 记录类型数据层
    /// </summary>
    public sealed class SingDevelopTypeService : BaseService, ISingDevelopType
    {
        #region 单例模式

        private static SingDevelopTypeService _singDevelopTypeService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingDevelopTypeService Instance
        {
            get
            {
                if (_singDevelopTypeService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singDevelopTypeService == null)
                        {
                            _singDevelopTypeService = new SingDevelopTypeService();
                        }
                    }
                }
                return _singDevelopTypeService;
            }
        }

        #endregion 单例模式

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
                DevelopType type = _db.DevelopTypes.SingleOrDefault(s => s.TypeId == id);
                return type;
            });
        }

        /// <summary>
        /// parentId 获取类型对象
        /// </summary>
        /// <param name="parentId">parentId</param>
        /// <returns>DevelopType</returns>
        public DevelopType GetDevelopTypeByParentId(int parentId)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                DevelopType type = _db.DevelopTypes.SingleOrDefault(s => s.TypeId == parentId);
                return type;
            });
        }

        /// <summary>
        /// 获取类型集合
        /// </summary>
        public IList<DevelopType> GetDevelopTypeList()
        {
            return ExecuteReturnEntitysMethod((_db) =>
            {
                return _db.DevelopTypes.OrderByDescending(o => o.UpdatedTime).ThenBy(t => t.Name).ToListAsync().Result;
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
    }
}
