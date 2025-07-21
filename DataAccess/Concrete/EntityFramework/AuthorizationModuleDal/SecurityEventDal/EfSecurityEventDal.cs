using Core.Database.Base;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories;
using DataAccess.Database.Context;
using Domain.Entities.MainEntities.AuthorizationModuleEntities.SecurityEvents;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.AuthorizationModuleDal.SecurityEventDal
{
    public class EfSecurityEventDal : EfGenericRepositoryBase<SecurityEvent, UnifeContext>, ISecurityEventRepository
    {
        public EfSecurityEventDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterAdminUuidAsync(Guid executerAdminUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventedByAdminUuid == executerAdminUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStaffUuidAsync(Guid executerStaffUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventedByStaffUuid == executerStaffUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventExecuterStudentUuidAsync(Guid executerStudentUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventedByStudentUuid == executerStudentUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTimeAsync(DateTime eventTime)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventTime == eventTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventTime >= startTime && se.EventTime <= endTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByEventTypeUuidAsync(Guid eventTypeUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.EventTypeUuid == eventTypeUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByIpAddressAsync(string ipAddress)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.IpAddress == ipAddress)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByUniversityUuidAsync(Guid universityUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.UniversityUuid == universityUuid)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllByUserAgentAsync(string userAgent)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.UserAgent == userAgent)
                .ToListAsync();
        }

        public async Task<IEnumerable<SecurityEvent>> GetAllUniversityUuidAsync(Guid universityUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .Where(se => se.UniversityUuid == universityUuid)
                .ToListAsync();
        }

        public async Task<SecurityEvent?> GetByUuid(Guid eventUuid)
        {
            using var context = _connectionFactory.CreateContext();
            return await context.SecurityEvents
                .FirstOrDefaultAsync(se => se.SecurityEventUuid == eventUuid);
        }
    }
}
