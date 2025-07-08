using Core.Database.Base;
using Domain.Entities.Base.Abstract;
using Domain.Repositories.Abstract.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Concrete.EntityFramework
{
    public class EfGenericRepositoryBase<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        protected readonly IDbConnectionFactory<TContext> _connectionFactory;

        protected EfGenericRepositoryBase(IDbConnectionFactory<TContext> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> CountWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<TEntity>().CountAsync(predicate);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            
            // Entity'nin context'e attach edilip edilmediğini kontrol et
            var entry = context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                context.Set<TEntity>().Attach(entity);
            }
            
            context.Set<TEntity>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetPagedAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int skip = 0, int take = 10)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<TResult> UseQueryableAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> queryFunc)
        {
            await using var context = await _connectionFactory.CreateContextAsync();
            var queryable = context.Set<TEntity>().AsQueryable();
            return await queryFunc(queryable);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> SaveChangesAsync()
        {
            try 
            {
                using var context = _connectionFactory.CreateContextAsync().Result;
                return await Task.FromResult(context.SaveChanges() > 0);
            }
            catch (Exception)
            {
                // Log exception or handle it as needed
                return await Task.FromResult(false);
            }
        }
    }
}
