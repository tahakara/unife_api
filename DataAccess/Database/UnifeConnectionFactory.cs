using Core.Database.Base;
using DataAccess.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Database
{
    public class UnifeConnectionFactory : DbConnectionFactoryBase<UnifeContext>
    {
        public UnifeConnectionFactory(IConfiguration configuration) 
            : base(configuration, "UnifeDatabase") // Istediginiz key
        {
        }

        protected override void ConfigureContext(DbContextOptionsBuilder<UnifeContext> optionsBuilder)
        {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }

        protected override UnifeContext CreateContextInstance(DbContextOptions<UnifeContext> options)
        {
            return new UnifeContext(options);
        }
    }
}