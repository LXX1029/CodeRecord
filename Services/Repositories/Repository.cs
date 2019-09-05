using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using Services.EFCodeFirst;

namespace Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        public Repository()
        {

        }
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected RecordContext DbContext { get; } = DbContextFactory.Instance;
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="t">实体对象</param>
        public virtual async Task<T> AddEntity(T t)
        {
            T entity = DbContext.Set<T>().Add(t);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id">主键</param>
        public virtual async Task<T> GetEntity(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);

        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体对象</param>
        public virtual async Task<T> UpdateEntity(T t)
        {
            if (t == null)
                return null;
            if (DbContext.Entry<T>(t).State != EntityState.Modified)
            {
                DbContext.Entry<T>(t).State = EntityState.Modified;
            }
            await DbContext.SaveChangesAsync();
            return t;
        }
        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">主键</param>
        public virtual async Task<bool> RemoveEntity(int id)
        {
            T entity = await DbContext.Set<T>().FindAsync(id);
            if (entity != null)
                DbContext.Set<T>().Remove(entity);
            return await DbContext.SaveChangesAsync() > 0 ? true : false;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="t">实体对象</param>
        public virtual async Task<bool> RemoveEntity(T t)
        {
            if (t != null)
                DbContext.Set<T>().Remove(t);
            return await DbContext.SaveChangesAsync() > 0 ? true : false;
        }
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual async Task<List<T>> GetEntities(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                return await DbContext.Set<T>().ToListAsync();
            return await DbContext.Set<T>().Where(predicate).ToListAsync();
        }
        /// <summary>
        /// 获取实体集
        /// </summary>
        /// <param name="predicate">条件</param>
        public virtual IQueryable<T> GetQueryableEntities(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
                return DbContext.Set<T>();
            return DbContext.Set<T>().Where(predicate);
        }
    }
}
