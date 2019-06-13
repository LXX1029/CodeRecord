using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static Common.ExceptionHelper;

namespace Services.EF
{
    public abstract class BaseService
    {
        #region EF实体

        //private object _obj;
        /// <summary>
        /// EF 实体对象
        /// </summary>
        protected RecordDBEntities _db;

        /// <summary>
        /// 当前DbContext事物引用
        /// </summary>
        private DbContextTransaction _tran;

        #endregion EF实体

        #region 返回 hSFAFCDb 更新结果

        /// <summary>
        /// 返回 hSFAFCDb 更新结果
        /// </summary>
        /// <returns>bool</returns>
        public bool ReturnSaveChanges()
        {
            if (_db == null)
                return false;

            return _db.SaveChanges() > 0 ? true : false;
        }

        #endregion 返回 hSFAFCDb 更新结果

        #region Public Methods
        /// <summary>
        /// 执行新增，修改，删除操作
        /// </summary>
        /// <param name="action">执行的方法体</param>
        public bool ExecuteReturnBoolMethod(Action<RecordDBEntities> action = null)
        {
            bool changeResult = false;
            CatchException(() =>
            {
                using (_db = new RecordDBEntities())
                {
                    _tran = _db.Database.BeginTransaction();
                    action?.Invoke(_db);
                    changeResult = ReturnSaveChanges();
                    _tran?.Commit();
                }
            }, (ex) =>
            {
                _tran?.Rollback();
            }, () =>
            {
                _tran?.Dispose();
            });
            return changeResult;
        }

        /// <summary>
        /// 执行查询单个实体
        /// </summary>
        /// <typeparam name="T">要返回的实体对象类型</typeparam>
        /// <param name="action">执行的方法体</param>
        /// <returns>实体对象类型</returns>
        public T ExecuteReturnEntityMethod<T>(Func<RecordDBEntities, T> action)
             where T : class
        {
            T t = default(T);
            using (_db = new RecordDBEntities())
            {
                t = action.Invoke(_db);
            }
            return t;
        }

        /// <summary>
        /// 执行查询实体集合
        /// </summary>
        /// <typeparam name="T">实体集合类型</typeparam>
        /// <param name="action">执行的方法体</param>
        /// <returns>实体集合</returns>
        public IList<T> ExecuteReturnEntitysMethod<T>(Func<RecordDBEntities, IList<T>> action)
            where T : class
        {
            IList<T> ts = new List<T>();
            using (_db = new RecordDBEntities())
            {
                ts = action?.Invoke(_db);
            }
            return ts;
        }

        public IQueryable<T> ExecuteReturnEntitysMethod<T>(Func<RecordDBEntities, IQueryable<T>> action)
          where T : class
        {
            IQueryable<T> ts = null;
            using (_db = new RecordDBEntities())
            {
                ts = action?.Invoke(_db);
            }
            return ts;
        }

        /// <summary>
        /// 执行返回单个结果操作
        /// </summary>
        /// <param name="action">执行的方法体</param>
        public T ExecuteReturnObjectMethod<T>(Func<RecordDBEntities, T> action)
        {
            T t = default(T);
            using (_db = new RecordDBEntities())
            {
                t = action.Invoke(_db);
            }
            return t;
        }

        /// <summary>
        /// 执行sql语句返回操作结果
        /// </summary>
        /// <param name="sql">执行的sql</param>
        /// <returns>bool</returns>
        public bool ExecuteSqlCommandReturnObject(string sql)
        {
            bool changeResult = false;
            CatchException(() =>
            {
                using (_db = new RecordDBEntities())
                {
                    _tran = _db.Database.BeginTransaction();
                    changeResult = (_db.Database.ExecuteSqlCommand(sql) > 0 ? true : false);
                    _tran?.Commit();
                }
            }, (ex) =>
            {
                _tran?.Rollback();
            }, () =>
            {
                _tran?.Dispose();
            });
            return changeResult;
        }

        /// <summary>
        /// 执行sql语句返回单个类型 
        /// 该方法为泛型方法
        /// </summary>
        /// <param name="T">返回的实体</param>
        /// <param name="sql">执行的sql</param>
        /// <returns>实体</returns>
        public T ExecuteSqlQueryReturnObject<T>(string sql)
        {
            T t = default(T);
            using (_db = new RecordDBEntities())
            {
                t = _db.Database.SqlQuery<T>(sql).SingleOrDefaultAsync().Result;
            }
            return t;
        }

        /// <summary>
        /// 执行sql语句返回结果集
        /// </summary>
        /// <typeparam name="T">返回的实体</typeparam>
        /// <param name="sql">执行的sql</param>
        /// <returns>实体结果集</returns>
        public IList<T> ExecuteSqlQueryReturnObjects<T>(string sql)
             where T : class
        {
            IList<T> ts = new List<T>();
            using (_db = new RecordDBEntities())
            {
                ts = _db.Database.SqlQuery<T>(sql).ToListAsync().Result;
            }
            return ts;
        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// 释放资源
        /// </summary>
        private void Dispose()
        {
            _db?.Dispose();
            _tran?.Dispose();
        }

        #endregion Private Methods
    }
}