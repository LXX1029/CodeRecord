namespace Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="t">实体对象</param>
        Task<T> AddEntity(T t);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体对象</param>
        Task<T> UpdateEntity(T t);

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">主键</param>
        Task<int> RemoveEntity(int id);

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="t">实体对象</param>
        Task<int> RemoveEntity(T t);

        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id">主键</param>
        Task<T> GetEntity(int id);

        /// <summary>
        /// 获取实体集 Task<List<T>>
        /// </summary>
        /// <param name="predicate">条件</param>
        Task<List<T>> GetEntities(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 获取实体集 IQueryable<T>
        /// </summary>
        /// <param name="predicate">条件</param>
        IQueryable<T> GetQueryableEntities(Expression<Func<T, bool>> predicate = null);
    }
}
