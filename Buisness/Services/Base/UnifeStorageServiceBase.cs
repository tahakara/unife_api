using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Buisness.Services.Base
{
    /// <summary>
    /// Unife projesi için generic storage base service
    /// </summary>
    public abstract class UnifeStorageServiceBase
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger _logger;
        private readonly string _storageKey;

        protected UnifeStorageServiceBase(
            IServiceProvider serviceProvider, 
            ILogger logger, 
            string storageKey)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _storageKey = storageKey;
        }

        protected async Task<IObjectStorageConnection> GetConnectionAsync()
        {
            var factory = _serviceProvider.GetRequiredKeyedService<IObjectStorageConnectionFactory>(_storageKey);
            return await factory.CreateConnectionAsync();
        }

        protected virtual TimeSpan GetDefaultExpiration()
        {
            return _storageKey switch
            {
                "cache" => TimeSpan.FromMinutes(15),
                "session" => TimeSpan.FromMinutes(30),
                _ => TimeSpan.FromMinutes(15)
            };
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var value = await connection.GetStringAsync(key);
                
                if (string.IsNullOrEmpty(value))
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get value for key: {Key} from {StorageType}", key, _storageKey);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var serializedValue = JsonSerializer.Serialize(value);
                
                var defaultExpiration = expiration ?? GetDefaultExpiration();
                await connection.SetStringAsync(key, serializedValue, defaultExpiration);
                
                _logger.LogDebug("Value set for key: {Key} in {StorageType}", key, _storageKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set value for key: {Key} in {StorageType}", key, _storageKey);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                await connection.DeleteAsync(key);
                
                _logger.LogDebug("Value removed for key: {Key} from {StorageType}", key, _storageKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove value for key: {Key} from {StorageType}", key, _storageKey);
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var keys = await connection.GetKeysAsync(pattern);
                
                if (keys.Any())
                {
                    await connection.DeleteBatchAsync(keys);
                    _logger.LogInformation("Pattern removed: {Pattern} from {StorageType}, Count: {Count}", 
                        pattern, _storageKey, keys.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove pattern: {Pattern} from {StorageType}", pattern, _storageKey);
            }
        }
    }
}