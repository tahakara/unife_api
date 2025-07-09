using Buisness.Abstract.ServicesBase;
using Buisness.Services.Base;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.UtilityServices
{
    public class UnifeCacheService : UnifeStorageServiceBase, ICacheService
    {
        public UnifeCacheService(IServiceProvider serviceProvider, ILogger<UnifeCacheService> logger)
            : base(serviceProvider, logger, "cache")
        {
        }

        public async Task InvalidateEntityCachesAsync(string entityName)
        {
            var pattern = $"*{entityName}*";
            await RemoveByPatternAsync(pattern);
        }

        public async Task InvalidateUniversityCachesAsync()
        {
            await InvalidateEntityCachesAsync("University");
        }

        public async Task InvalidateFacultyCachesAsync()
        {
            await InvalidateEntityCachesAsync("Faculty");
        }

        public async Task InvalidateUniversityByIdAsync(Guid universityId)
        {
            var pattern = $"*University*{universityId}*";
            await RemoveByPatternAsync(pattern);
        }

        public async Task InvalidateAllEntitiesCacheAsync()
        {
            var entityTypes = new[] { "University", "Faculty", "Academician", "Department" };
            
            var tasks = entityTypes.Select(InvalidateEntityCachesAsync);
            await Task.WhenAll(tasks);
        }
    }
}