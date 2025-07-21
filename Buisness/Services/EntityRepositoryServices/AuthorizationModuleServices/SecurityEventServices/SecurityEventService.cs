using AutoMapper;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Concrete.ServiceManager;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices.SecurityEventServices
{
    public class SecurityEventService : ServiceManagerBase, ISecurityEventService
    {
        private readonly ISecurityEventRepository _securityEventRepository;
        public SecurityEventService(
            ISecurityEventRepository SecurityEventRepository,
            IMapper mapper,
            ILogger<SecurityEventService> logger,
            IServiceProvider serviceProvider) :
            base(mapper, logger, serviceProvider)
        {
            _securityEventRepository = SecurityEventRepository;
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterAdminUuidAsync(Guid executerAdminUuid)
        {
            return await _securityEventRepository.GetAllByEventExecuterAdminUuidAsync(executerAdminUuid);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStaffUuidAsync(Guid executerStaffUuid)
        {
            return await _securityEventRepository.GetAllByEventExecuterStaffUuidAsync(executerStaffUuid);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStudentUuidAsync(Guid executerStudentUuid)
        {
            return await _securityEventRepository.GetAllByEventExecuterStudentUuidAsync(executerStudentUuid);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTimeAsync(DateTime eventTime)
        {
            return await _securityEventRepository.GetAllByEventTimeAsync(eventTime);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            return await _securityEventRepository.GetAllByEventTimeRangeAsync(startTime, endTime);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTypeUuidAsync(Guid eventTypeUuid)
        {
            return await _securityEventRepository.GetAllByEventTypeUuidAsync(eventTypeUuid);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByIpAddressAsync(string ipAddress)
        {
            return await _securityEventRepository.GetAllByIpAddressAsync(ipAddress);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByUniversityUuidAsync(Guid universityUuid)
        {
            return await _securityEventRepository.GetAllByUniversityUuidAsync(universityUuid);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByUserAgentAsync(string userAgent)
        {
            return await _securityEventRepository.GetAllByUserAgentAsync(userAgent);
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllUniversityUuidAsync(Guid universityUuid)
        {
            return await _securityEventRepository.GetAllUniversityUuidAsync(universityUuid);
        }

        public async Task<SecurityEvent?> GetByUuid(Guid eventUuid)
        {
            return await _securityEventRepository.GetByUuid(eventUuid);
        }

        public async Task<bool> RecordSecurityEventAsync(SecurityEvent securityEvent)
        {
            return await _securityEventRepository.AddAsync(securityEvent) != null;
        }
    }
}
