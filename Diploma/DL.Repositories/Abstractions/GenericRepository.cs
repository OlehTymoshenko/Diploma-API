using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DL.Entities.Base;
using DL.Interfaces.Repositories.Abstractions;

namespace DL.Repositories.Abstractions
{
    abstract class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
            
        // TODO : Add private field for DbContext and contructor for injecting this context

        public async Task<int> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
