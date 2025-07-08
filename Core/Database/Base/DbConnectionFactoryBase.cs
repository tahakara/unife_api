using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Database.Base
{
    public abstract class DbConnectionFactoryBase<TContext> : IDbConnectionFactory<TContext> 
        where TContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        protected readonly string _connectionStringKey;

        protected DbConnectionFactoryBase(IConfiguration configuration, string connectionStringKey)
        {
            _configuration = configuration;
            _connectionStringKey = connectionStringKey;
        }

        public virtual TContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            ConfigureContext(optionsBuilder);
            
            // Reflection ile context olustur veya factory pattern kullan
            return CreateContextInstance(optionsBuilder.Options);
        }

        public virtual Task<TContext> CreateContextAsync()
        {
            return Task.FromResult(CreateContext());
        }

        protected abstract void ConfigureContext(DbContextOptionsBuilder<TContext> optionsBuilder);
        protected abstract TContext CreateContextInstance(DbContextOptions<TContext> options);

        protected string GetConnectionString()
        {
            return _configuration.GetConnectionString(_connectionStringKey) 
                ?? throw new InvalidOperationException($"Connection string '{_connectionStringKey}' not found.");
        }
    }
}