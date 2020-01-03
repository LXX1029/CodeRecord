using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Services.EFCodeFirst;

namespace Services.CastleWindsor
{
    public class Repository : IRepository
    {
        public Repository()
        { }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        // protected RecordContext DbContext { get; } = DbContextFactory.Instance;

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<T> AddEntity<T>(T t) where T : class
        {
            using (RecordContext context = new RecordContext())
            {
                T entity = context.Set<T>().Add(t);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id">主键</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<T> GetEntity<T>(int id) where T : class
        {
            using (RecordContext context = new RecordContext())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<T> UpdateEntity<T>(T t) where T : class
        {
            if (t == null) return null;
            using (RecordContext context = new RecordContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (context.Entry<T>(t).State != EntityState.Modified)
                            context.Entry<T>(t).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
                return t;
            }
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<int> RemoveEntity<T>(int id) where T : class
        {
            var affectedRows = 0;
            if (id < 0) return affectedRows;
            using (RecordContext context = new RecordContext())
            {
                var entity = await context.Set<T>().FindAsync(id);
                if (entity == null) return affectedRows;
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Set<T>().Remove(entity);
                        affectedRows = await context.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<int> RemoveEntity<T>(T t) where T : class
        {
            var affectedRows = 0;
            if (t == null) return affectedRows;
            using (RecordContext context = new RecordContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Set<T>().Remove(t);
                        affectedRows = await context.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return affectedRows;
        }

        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <typeparam name="T">T</typeparam>
        public virtual async Task<List<T>> GetEntities<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (RecordContext context = new RecordContext())
            {
                if (predicate == null)
                    return await context.Set<T>().ToListAsync();
                return await context.Set<T>().Where(predicate).ToListAsync();
            }
        }

        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <typeparam name="T">T</typeparam>
        public virtual IQueryable<T> GetQueryableEntities<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            using (var context = new RecordContext())
            {
                if (predicate == null)
                    return context.Set<T>();
                return context.Set<T>().Where(predicate);
            }
        }
    }
}
