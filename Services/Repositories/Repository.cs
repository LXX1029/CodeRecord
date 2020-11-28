using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Services.EFCodeFirst;

namespace Services.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
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
        public virtual async Task<T> AddEntity(T t)
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
        public virtual async Task<T> GetEntity(int id)
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
        public virtual async Task<T> UpdateEntity(T t)
        {
            if (t == null) return null;
            using (var context = new RecordContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var entry1 = context.Entry<T>(t);
                        if (context.Entry<T>(t).State != EntityState.Modified)
                            context.Entry<T>(t).State = EntityState.Modified;
                        var entry = context.Entry<T>(t);
                        context.Database.Log = log =>
                        {
                            Console.WriteLine(log);
                        };
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
        public virtual async Task<int> RemoveEntity(int id)
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
        public virtual async Task<int> RemoveEntity(T t)
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
        public virtual async Task<List<T>> GetEntities(Expression<Func<T, bool>> predicate)
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
        public virtual IQueryable<T> GetQueryableEntities(Expression<Func<T, bool>> predicate = null)
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
