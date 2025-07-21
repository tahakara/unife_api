using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Repositories.Abstract.Base;

namespace DataAccess.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories
{
    public interface ISecurityEventTypeRepository : IGenericRepository<SecurityEventType>
    {   
        Task<SecurityEventType?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<SecurityEventType>> GetAllAsync(bool isDeleted = false);
        Task<bool> IsNameExistsAsync(string name, bool isDeleted = false);
        Task<bool> IsSystemEventAsync(Guid uuid);
    }
}
