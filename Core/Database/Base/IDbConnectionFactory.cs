using Microsoft.EntityFrameworkCore;

namespace Core.Database.Base
{
    public interface IDbConnectionFactory<TContext> where TContext : DbContext
    {
        TContext CreateContext();
        Task<TContext> CreateContextAsync();
    }
}