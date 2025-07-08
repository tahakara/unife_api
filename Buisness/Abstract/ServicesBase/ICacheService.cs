namespace Buisness.Abstract.ServicesBase
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
        Task InvalidateUniversityCachesAsync();
        Task InvalidateFacultyCachesAsync();
        
        // Yeni spesifik invalidation methodları
        Task InvalidateUniversityByIdAsync(Guid universityId);
        Task InvalidateAllEntitiesCacheAsync();
    }
}