namespace Services.CastleWindsor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        Task<T> AddEntity<T>(T t)
            where T : class;

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        Task<T> UpdateEntity<T>(T t)
            where T : class;

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <typeparam name="T">T</typeparam>
        Task<int> RemoveEntity<T>(int id) where T : class;

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <typeparam name="T">T</typeparam>
        Task<int> RemoveEntity<T>(T t) where T : class;

        /// <summary>
        /// 根据主键获取Entity
        /// </summary>
        /// <param name="id">主键</param>
        /// <typeparam name="T">T</typeparam>
        Task<T> GetEntity<T>(int id) where T : class;

        /// <summary>
        /// 获取实体集 Task<List<T>>
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <typeparam name="T">T</typeparam>
        Task<List<T>> GetEntities<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        /// <summary>
        /// 获取实体集 IQueryable<T>
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <typeparam name="T">T</typeparam>
        IQueryable<T> GetQueryableEntities<T>(Expression<Func<T, bool>> predicate = null) where T : class;
    }
}
