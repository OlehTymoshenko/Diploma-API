using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DL.Entities.Base;
using DL.Interfaces.Repositories.Abstractions;
using DL.EF.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using DL.Entities;

namespace DL.Repositories.Abstractions
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly ApplicationDbContext _appDbContext;

        public GenericRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<TEntity> GetByIdAsync(long id)
        {
            return _appDbContext.Set<TEntity>().FindAsync(id).AsTask();
        }

        public async Task<IEnumerable<TEntity>> SelectAsync()
        {
            return await _appDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _appDbContext.Set<TEntity>().AsQueryable();
            query = AddIncludesToQuery(query, includes);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _appDbContext.Set<TEntity>().Where(predicate);
            query = AddIncludesToQuery(query, includes);

            return await query.ToListAsync();
        }

        public Task AddAsync(TEntity entity)
        {
            return _appDbContext.Set<TEntity>().AddAsync(entity).AsTask();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return _appDbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public TEntity Update(TEntity entity)
        {
            return _appDbContext.Set<TEntity>().Update(entity).Entity;
        }

        public TEntity Delete(TEntity entity)
        {
            return _appDbContext.Set<TEntity>().Remove(entity).Entity;
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _appDbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _appDbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _appDbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _appDbContext.Set<TEntity>().AsQueryable();
            query = AddIncludesToQuery(query, includes);

            return query.SingleOrDefaultAsync(predicate);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _appDbContext.Set<TEntity>().CountAsync(predicate);
        }


        private static IQueryable<TEntity> AddIncludesToQuery(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            includes?.ToList().ForEach(e => query = query.Include(e));

            return query;
        }
    }
}
