using Buisness.Abstract.ServicesBase;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Buisness.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public Task<T?> GetAsync<T>(string key) where T : class
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(5),
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(key, value, options);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPatternAsync(string pattern)
        {
            var cacheKeysToRemove = GetCacheKeysByPattern(pattern);
            
            foreach (var key in cacheKeysToRemove)
            {
                _cache.Remove(key);
                _logger.LogInformation("Cache pattern ile temizlendi: {CacheKey}", key);
            }

            _logger.LogInformation("Pattern '{Pattern}' ile {Count} cache entry temizlendi", pattern, cacheKeysToRemove.Count);
            return Task.CompletedTask;
        }

        public Task InvalidateUniversityCachesAsync()
        {
            return RemoveByPatternAsync(".*University.*");
        }

        public Task InvalidateFacultyCachesAsync()
        {
            return RemoveByPatternAsync(".*Faculty.*");
        }

        public Task InvalidateUniversityByIdAsync(Guid universityId)
        {
            // Belirli bir üniversite ID'si için cache'leri temizle
            var pattern = $".*University.*{universityId}.*";
            return RemoveByPatternAsync(pattern);
        }

        public Task InvalidateAllEntitiesCacheAsync()
        {
            // Tüm entity cache'lerini temizle
            var patterns = new[]
            {
                ".*University.*",
                ".*Faculty.*",
                ".*Academician.*",
                ".*Department.*"
            };

            var tasks = patterns.Select(pattern => RemoveByPatternAsync(pattern));
            return Task.WhenAll(tasks);
        }

        private List<string> GetCacheKeysByPattern(string pattern)
        {
            var cacheKeys = new List<string>();
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var field = typeof(MemoryCache).GetField("_coherentState", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (field?.GetValue(_cache) is not object coherentState) return cacheKeys;

            var entriesCollection = coherentState.GetType()
                .GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(coherentState);

            if (entriesCollection is not IDictionary entries) return cacheKeys;

            foreach (DictionaryEntry entry in entries)
            {
                if (entry.Key is string key && regex.IsMatch(key))
                {
                    cacheKeys.Add(key);
                }
            }

            return cacheKeys;
        }
    }
}