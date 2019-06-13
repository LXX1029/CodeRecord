using Services.EF;
using Services.Singleton.ISingRecord;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Singleton.SingRecord
{
    /// <summary>
    /// 用户模块数据层
    /// </summary>
    public sealed class SingUserService : BaseService, ISingUser
    {
        #region 单例模式

        private static SingUserService _singUserService;
        private static object _obj;

        /// <summary>
        /// 获取单例
        /// </summary>
        public static SingUserService Instance
        {
            get
            {
                if (_singUserService == null)
                {
                    _obj = new object();
                    lock (_obj)
                    {
                        if (_singUserService == null)
                        {
                            _singUserService = new SingUserService();
                        }
                    }
                }
                return _singUserService;
            }
        }

        #endregion 单例模式

        #region 用户管理
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">DevelopUser</param>
        /// <returns>bool</returns>
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

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user">DevelopUser</param>
        /// <returns>bool</returns>
        public bool DeleteUser(DevelopUser user)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                _db.Entry(user).State = EntityState.Deleted;
                _db.DevelopUsers.Remove(user);
            });
        }

        /// <summary>
        /// 根据Id删除用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>bool</returns>
        public bool DeleteUserByUserId(int userId)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                DevelopUser user = _db.DevelopUsers.SingleOrDefault(s => s.UserId == userId);
                _db.DevelopUsers.Remove(user);
            });
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>bool</returns>
        public bool UpdateUser(DevelopUser user)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                // 非同一上下文的对象要设置其状态为修改状态
                _db.Entry(user).State = EntityState.Modified;
            });
        }
        /// <summary>
        /// 根据用户名，密码获取用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns>DevelopUser</returns>
        public DevelopUser GetDevelopUser(string name, string pwd)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopUsers.Include("DevelopPowerFuns.DevelopFun").SingleOrDefaultAsync(s => s.Name == name && s.Pwd == pwd).Result;
            });
        }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>DevelopUser</returns>
        public DevelopUser GetDevelopUserByUserId(int userId)
        {
            return ExecuteReturnEntityMethod((_db) =>
            {
                return _db.DevelopUsers.SingleOrDefaultAsync(s => s.UserId == userId).Result;
            });
        }

        /// <summary>
        /// 根据Id获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>DevelopUser</returns>
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
    }
}
