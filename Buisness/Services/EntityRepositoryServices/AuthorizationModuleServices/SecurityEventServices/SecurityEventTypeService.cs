using AutoMapper;
using Buisness.Services.EntityRepositoryServices.Base;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices.SecurityEventServices;
using Core.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices.SecurityEventServices
{
    public class SecurityEventTypeService : ServiceManagerBase, ISecurityEventTypeService
    {
        private readonly ISecurityEventTypeRepository _securityEventTypeRepository;
        public SecurityEventTypeService(
            ISecurityEventTypeRepository SecurityEventTypeRepository,
            IMapper mapper,
            ILogger<SecurityEventTypeService> logger,
            IServiceProvider serviceProvider) :
            base(mapper, logger, serviceProvider)
        {
            _securityEventTypeRepository = SecurityEventTypeRepository;
        }

        public async Task<IEnumerable<SecurityEventType>> GetAllAsync(bool isDeleted = false)
        {
            return await _securityEventTypeRepository.GetAllAsync(isDeleted);
        }

        public async Task<SecurityEventType?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            return await _securityEventTypeRepository.GetByUuidAsync(uuid, isDeleted);
        }

        public async Task<bool> IsNameExistsAsync(string name, bool isDeleted = false)
        {
            return await _securityEventTypeRepository.IsNameExistsAsync(name, isDeleted);
        }

        public async Task<bool> IsSystemEventAsync(Guid uuid)
        {
            return await _securityEventTypeRepository.IsSystemEventAsync(uuid);
        }
    }
}
