using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Repositories.Abstract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories
{
    public interface ISecurityEventRepository : IGenericRepository<SecurityEvent>
    {
        Task<SecurityEvent?> GetByUuid(Guid eventUuid);
        Task<IEnumerable<SecurityEvent>> GetAllUniversityUuidAsync(Guid universityUuid);
        Task<IEnumerable<SecurityEvent>> GetAllByUniversityUuidAsync(Guid universityUuid);
        Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterAdminUuidAsync(Guid executerAdminUuid);
        Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStaffUuidAsync(Guid executerStaffUuid);
        Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStudentUuidAsync(Guid executerStudentUuid);

        Task<IEnumerable<SecurityEvent>> GetAllByEventTypeUuidAsync(Guid eventTypeUuid);

        Task<IEnumerable<SecurityEvent>> GetAllByEventTimeAsync(DateTime eventTime);
        Task<IEnumerable<SecurityEvent>> GetAllByEventTimeRangeAsync(DateTime startTime, DateTime endTime);
        Task<IEnumerable<SecurityEvent>> GetAllByIpAddressAsync(string ipAddress);
        Task<IEnumerable<SecurityEvent>> GetAllByUserAgentAsync(string userAgent);
    }
}
