using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DL.Entities.Base;

namespace DL.Interfaces.Repositories.Abstractions
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<int> Delete(T entity);
        Task<int> Update(T entity);
    }
}
