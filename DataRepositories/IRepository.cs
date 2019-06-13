using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetEntity(int id);
        Task<T> AddEntity(T t);
        Task<T> UpdateEntity(T t);
        Task<bool> DeleteEntity(int id);
    }
}
