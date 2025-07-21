using Core.Database.Base;
using Core.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories;
using Core.Database.Context;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Core.Concrete.EntityFramework.AuthorizationModuleDal.SecurityEventDal
{
    public class EfSecurityEventTypeDal : EfGenericRepositoryBase<SecurityEventType, UnifeContext>, ISecurityEventTypeRepository
    {
        public EfSecurityEventTypeDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<SecurityEventType>> GetAllAsync(bool isDeleted = false)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEventTypes
                .Where(set => set.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<SecurityEventType?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEventTypes
                .Where(set => set.SecurityEventTypeUuid == uuid && set.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name, bool isDeleted = false)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEventTypes
                .AnyAsync(set => set.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && set.IsDeleted == isDeleted);
        }

        public async Task<bool> IsSystemEventAsync(Guid uuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEventTypes
                .AnyAsync(set => set.SecurityEventTypeUuid == uuid && set.IsSystemEvent);
        }
    }
}
