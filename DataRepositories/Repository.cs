using Services.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RecordDBEntities dbContext;
        public Repository()
        {
            dbContext = new RecordDBEntities();
        }

        public Task<T> AddEntity(T t)
        {
            
            return null;
        }

        public Task<bool> DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateEntity(T t)
        {
            throw new NotImplementedException();
        }
    }
}
