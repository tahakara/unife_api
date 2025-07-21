using Buisness.Abstract.ServicesBase.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;

namespace Buisness.Abstract.ServicesBase.AuthorizationModuleServices.SecurityEventServices
{
    public interface ISecurityEventTypeService : IServiceManagerBase
    {
        Task<SecurityEventType?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<SecurityEventType>> GetAllAsync(bool isDeleted = false);
        Task<bool> IsNameExistsAsync(string name, bool isDeleted = false);
        Task<bool> IsSystemEventAsync(Guid uuid);
    }
}
