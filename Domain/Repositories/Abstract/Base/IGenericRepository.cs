using Core.Entities.Base.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Abstract.Base
{
    public interface IGenericRepository<T> where T : class, IEntity, new()
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<TResult> UseQueryableAsync<TResult>(Func<IQueryable<T>, Task<TResult>> queryFunc);

        // Pagination methods
        Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int skip = 0, int take = 10);
        Task<int> CountWhereAsync(Expression<Func<T, bool>> predicate);
        Task<bool> SaveChangesAsync();
    }
}
