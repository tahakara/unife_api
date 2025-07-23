using Buisness.Services.EntityRepositoryServices.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;

namespace Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices.SecurityEventServices
{
    public interface ISecurityEventTypeService : IServiceManagerBase
    {
        Task<SecurityEventType?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<SecurityEventType>> GetAllAsync(bool isDeleted = false);
        Task<bool> IsNameExistsAsync(string name, bool isDeleted = false);
        Task<bool> IsSystemEventAsync(Guid uuid);
    }
}
