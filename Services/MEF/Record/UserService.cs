using Services.MEF.IRecord;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Services.EF;

namespace Services.MEF.Record
{
    [Export("UserService", typeof(IUserService))]
    public class UserService : BaseService, IUserService
    {
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

        public bool UpdateUser(DevelopUser user)
        {
            return ExecuteReturnBoolMethod((_db) =>
            {
                // 非同一上下文的对象要设置其状态
                _db.Entry(user).State = EntityState.Modified;
            });
        }

        #endregion 用户管理
    }
}