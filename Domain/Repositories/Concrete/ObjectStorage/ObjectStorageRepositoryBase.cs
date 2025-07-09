using Core.ObjectStorage.Base;
using Domain.Repositories.Abstract.Base;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Reflection;

namespace Domain.Repositories.Concrete.ObjectStorage
{
    public class ObjectStorageRepositoryBase<T> : IObjectStorageRepository<T>, IDisposable where T : class
    {
        protected readonly IObjectStorageConnectionFactory _connectionFactory;
        protected readonly ILogger<ObjectStorageRepositoryBase<T>> _logger;
        protected readonly string _entityPrefix;
        private IObjectStorageConnection? _connection;

        protected ObjectStorageRepositoryBase(
            IObjectStorageConnectionFactory connectionFactory, 
            ILogger<ObjectStorageRepositoryBase<T>> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _entityPrefix = GetEntityPrefix();
        }

        #region Connection Management

        protected virtual async Task<IObjectStorageConnection> GetConnectionAsync()
        {
            if (_connection == null || !_connection.IsConnected)
            {
                _logger.LogDebug("Creating new object storage connection for {EntityType}", typeof(T).Name);
                _connection = await _connectionFactory.CreateConnectionAsync();
                _logger.LogDebug("Object storage connection established for {EntityType}", typeof(T).Name);
            }

            return _connection;
        }

        protected virtual string GetPrefixedKey(string key)
        {
            return $"{_entityPrefix}:{key}";
        }

        protected virtual string GetEntityPrefix()
        {
            return typeof(T).Name.ToLowerInvariant();
        }

        #endregion

        #region Basic CRUD Operations

        public virtual async Task<T?> GetAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var value = await connection.GetStringAsync(prefixedKey);
                
                if (value == null)
                {
                    _logger.LogDebug("Key not found: {Key}", prefixedKey);
                    return null;
                }

                var result = JsonSerializer.Deserialize<T>(value);
                _logger.LogDebug("Retrieved entity from key: {Key}", prefixedKey);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get entity from key: {Key}", key);
                return null;
            }
        }

        public virtual async Task<List<T>> GetAllAsync(string pattern = "*")
        {
            try
            {
                var keys = await GetKeysAsync(pattern);
                var entities = new List<T>();
                var connection = await GetConnectionAsync();
                
                foreach (var key in keys)
                {
                    var value = await connection.GetStringAsync(key);
                    if (value != null)
                    {
                        var entity = JsonSerializer.Deserialize<T>(value);
                        if (entity != null)
                        {
                            entities.Add(entity);
                        }
                    }
                }

                _logger.LogDebug("Retrieved {Count} entities with pattern: {Pattern}", entities.Count, pattern);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get entities with pattern: {Pattern}", pattern);
                return new List<T>();
            }
        }

        public virtual async Task<bool> SetAsync(string key, T entity, TimeSpan? expiry = null)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var serializedEntity = JsonSerializer.Serialize(entity);
                
                var result = await connection.SetStringAsync(prefixedKey, serializedEntity, expiry);
                
                _logger.LogDebug("Set entity to key: {Key}, Success: {Success}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set entity to key: {Key}", key);
                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(string key, T entity, TimeSpan? expiry = null)
        {
            // For object storage, update is the same as set
            return await SetAsync(key, entity, expiry);
        }

        public virtual async Task<bool> DeleteAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.DeleteAsync(prefixedKey);
                
                _logger.LogDebug("Deleted key: {Key}, Success: {Success}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete key: {Key}", key);
                return false;
            }
        }

        public virtual async Task<bool> ExistsAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.ExistsAsync(prefixedKey);
                
                _logger.LogDebug("Key exists check: {Key}, Exists: {Exists}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check key existence: {Key}", key);
                return false;
            }
        }

        #endregion

        #region JSON Operations

        public virtual async Task<bool> SetJsonAsync(string key, T entity, TimeSpan? expiry = null)
        {
            return await SetAsync(key, entity, expiry);
        }

        public virtual async Task<T?> GetJsonAsync(string key)
        {
            return await GetAsync(key);
        }

        #endregion

        #region Hash Operations

        public virtual async Task<bool> SetHashFieldAsync(string key, string field, string value)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.SetHashFieldAsync(prefixedKey, field, value);
                
                _logger.LogDebug("Set hash field: {Key}.{Field}, Success: {Success}", prefixedKey, field, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set hash field: {Key}.{Field}", key, field);
                return false;
            }
        }

        public virtual async Task<string?> GetHashFieldAsync(string key, string field)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var value = await connection.GetHashFieldAsync(prefixedKey, field);
                
                _logger.LogDebug("Get hash field: {Key}.{Field}, HasValue: {HasValue}", prefixedKey, field, value != null);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get hash field: {Key}.{Field}", key, field);
                return null;
            }
        }

        public virtual async Task<Dictionary<string, string>> GetAllHashFieldsAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.GetAllHashFieldsAsync(prefixedKey);
                
                _logger.LogDebug("Get all hash fields: {Key}, Count: {Count}", prefixedKey, result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all hash fields: {Key}", key);
                return new Dictionary<string, string>();
            }
        }

        public virtual async Task<bool> DeleteHashFieldAsync(string key, string field)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.DeleteHashFieldAsync(prefixedKey, field);
                
                _logger.LogDebug("Delete hash field: {Key}.{Field}, Success: {Success}", prefixedKey, field, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete hash field: {Key}.{Field}", key, field);
                return false;
            }
        }

        public virtual async Task<bool> SetEntityAsHashAsync(string key, T entity, TimeSpan? expiry = null)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                
                // Convert entity properties to hash fields
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var tasks = new List<Task<bool>>();
                
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(entity);
                    if (value != null)
                    {
                        var serializedValue = JsonSerializer.Serialize(value);
                        tasks.Add(connection.SetHashFieldAsync(prefixedKey, prop.Name, serializedValue));
                    }
                }
                
                var results = await Task.WhenAll(tasks);
                var success = results.All(r => r);
                
                if (success && expiry.HasValue)
                {
                    await connection.SetExpiryAsync(prefixedKey, expiry.Value);
                }
                
                _logger.LogDebug("Set entity as hash: {Key}, FieldCount: {Count}, Success: {Success}", 
                    prefixedKey, properties.Length, success);
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set entity as hash: {Key}", key);
                return false;
            }
        }

        public virtual async Task<T?> GetEntityFromHashAsync(string key)
        {
            try
            {
                var hashFields = await GetAllHashFieldsAsync(key);
                if (!hashFields.Any())
                {
                    return null;
                }

                var entity = Activator.CreateInstance<T>();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                foreach (var prop in properties)
                {
                    if (hashFields.TryGetValue(prop.Name, out var value))
                    {
                        var deserializedValue = JsonSerializer.Deserialize(value, prop.PropertyType);
                        prop.SetValue(entity, deserializedValue);
                    }
                }
                
                _logger.LogDebug("Retrieved entity from hash: {Key}", key);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get entity from hash: {Key}", key);
                return null;
            }
        }

        #endregion

        #region List Operations

        public virtual async Task<long> AddToListAsync(string key, T entity, bool leftPush = true)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var serializedEntity = JsonSerializer.Serialize(entity);
                
                var result = await connection.AddToListAsync(prefixedKey, serializedEntity, leftPush);
                
                _logger.LogDebug("Added to list: {Key}, Direction: {Direction}, NewLength: {Length}", 
                    prefixedKey, leftPush ? "Left" : "Right", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add to list: {Key}", key);
                return -1;
            }
        }

        public virtual async Task<T?> PopFromListAsync(string key, bool leftPop = true)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                
                var result = await connection.PopFromListAsync(prefixedKey, leftPop);
                
                if (result == null)
                {
                    return null;
                }
                
                var entity = JsonSerializer.Deserialize<T>(result);
                _logger.LogDebug("Popped from list: {Key}, Direction: {Direction}", 
                    prefixedKey, leftPop ? "Left" : "Right");
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to pop from list: {Key}", key);
                return null;
            }
        }

        public virtual async Task<List<T>> GetListRangeAsync(string key, long start = 0, long stop = -1)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var values = await connection.GetListRangeAsync(prefixedKey, start, stop);
                
                var entities = new List<T>();
                foreach (var value in values)
                {
                    var entity = JsonSerializer.Deserialize<T>(value);
                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                
                _logger.LogDebug("Get list range: {Key}, Start: {Start}, Stop: {Stop}, Count: {Count}", 
                    prefixedKey, start, stop, entities.Count);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get list range: {Key}", key);
                return new List<T>();
            }
        }

        public virtual async Task<long> GetListLengthAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var length = await connection.GetListLengthAsync(prefixedKey);
                
                _logger.LogDebug("Get list length: {Key}, Length: {Length}", prefixedKey, length);
                return length;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get list length: {Key}", key);
                return -1;
            }
        }

        #endregion

        #region Set Operations

        public virtual async Task<bool> AddToSetAsync(string key, T entity)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var serializedEntity = JsonSerializer.Serialize(entity);
                
                var result = await connection.AddToSetAsync(prefixedKey, serializedEntity);
                
                _logger.LogDebug("Add to set: {Key}, Added: {Added}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add to set: {Key}", key);
                return false;
            }
        }

        public virtual async Task<bool> RemoveFromSetAsync(string key, T entity)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var serializedEntity = JsonSerializer.Serialize(entity);
                
                var result = await connection.RemoveFromSetAsync(prefixedKey, serializedEntity);
                
                _logger.LogDebug("Remove from set: {Key}, Removed: {Removed}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove from set: {Key}", key);
                return false;
            }
        }

        public virtual async Task<bool> IsInSetAsync(string key, T entity)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var serializedEntity = JsonSerializer.Serialize(entity);
                
                var result = await connection.IsInSetAsync(prefixedKey, serializedEntity);
                
                _logger.LogDebug("Check set contains: {Key}, Contains: {Contains}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check set contains: {Key}", key);
                return false;
            }
        }

        public virtual async Task<List<T>> GetSetMembersAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var values = await connection.GetSetMembersAsync(prefixedKey);
                
                var entities = new List<T>();
                foreach (var value in values)
                {
                    var entity = JsonSerializer.Deserialize<T>(value);
                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                
                _logger.LogDebug("Get set members: {Key}, Count: {Count}", prefixedKey, entities.Count);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get set members: {Key}", key);
                return new List<T>();
            }
        }

        public virtual async Task<long> GetSetCountAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var count = await connection.GetSetCountAsync(prefixedKey);
                
                _logger.LogDebug("Get set count: {Key}, Count: {Count}", prefixedKey, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get set count: {Key}", key);
                return -1;
            }
        }

        #endregion

        #region Batch Operations

        public virtual async Task<Dictionary<string, T?>> GetBatchAsync(IEnumerable<string> keys)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKeys = keys.Select(GetPrefixedKey).ToList();
                var stringResults = await connection.GetBatchAsync(prefixedKeys);
                
                var result = new Dictionary<string, T?>();
                foreach (var kvp in stringResults)
                {
                    var originalKey = kvp.Key.StartsWith($"{_entityPrefix}:") 
                        ? kvp.Key.Substring($"{_entityPrefix}:".Length) 
                        : kvp.Key;
                    
                    if (kvp.Value != null)
                    {
                        var entity = JsonSerializer.Deserialize<T>(kvp.Value);
                        result[originalKey] = entity;
                    }
                    else
                    {
                        result[originalKey] = null;
                    }
                }
                
                _logger.LogDebug("Get batch: {Count} keys", keys.Count());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get batch");
                return new Dictionary<string, T?>();
            }
        }

        public virtual async Task<bool> SetBatchAsync(Dictionary<string, T> keyValuePairs, TimeSpan? expiry = null)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var stringPairs = new Dictionary<string, string>();
                
                foreach (var kvp in keyValuePairs)
                {
                    var prefixedKey = GetPrefixedKey(kvp.Key);
                    var serializedEntity = JsonSerializer.Serialize(kvp.Value);
                    stringPairs[prefixedKey] = serializedEntity;
                }
                
                var result = await connection.SetBatchAsync(stringPairs, expiry);
                
                _logger.LogDebug("Set batch: {Count} key-value pairs, Success: {Success}", keyValuePairs.Count, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set batch");
                return false;
            }
        }

        public virtual async Task<long> DeleteBatchAsync(IEnumerable<string> keys)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKeys = keys.Select(GetPrefixedKey);
                
                var result = await connection.DeleteBatchAsync(prefixedKeys);
                
                _logger.LogDebug("Delete batch: {InputCount} keys, {DeletedCount} deleted", keys.Count(), result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete batch");
                return 0;
            }
        }

        #endregion

        #region Search and Pattern Operations

        public virtual async Task<List<string>> GetKeysAsync(string pattern = "*")
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedPattern = GetPrefixedKey(pattern);
                var keys = await connection.GetKeysAsync(prefixedPattern);
                
                _logger.LogDebug("Get keys with pattern: {Pattern}, Count: {Count}", prefixedPattern, keys.Count);
                return keys;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get keys with pattern: {Pattern}", pattern);
                return new List<string>();
            }
        }

        public virtual async Task<List<T>> GetByPatternAsync(string pattern = "*")
        {
            try
            {
                var keys = await GetKeysAsync(pattern);
                var entities = new List<T>();
                
                foreach (var key in keys)
                {
                    var cleanKey = key.StartsWith($"{_entityPrefix}:") 
                        ? key.Substring($"{_entityPrefix}:".Length) 
                        : key;
                    var entity = await GetAsync(cleanKey);
                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                
                _logger.LogDebug("Get by pattern: {Pattern}, Count: {Count}", pattern, entities.Count);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get by pattern: {Pattern}", pattern);
                return new List<T>();
            }
        }

        public virtual async Task<long> CountByPatternAsync(string pattern = "*")
        {
            try
            {
                var keys = await GetKeysAsync(pattern);
                var count = keys.Count;
                
                _logger.LogDebug("Count by pattern: {Pattern}, Count: {Count}", pattern, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to count by pattern: {Pattern}", pattern);
                return 0;
            }
        }

        #endregion

        #region Expiry Operations

        public virtual async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.SetExpiryAsync(prefixedKey, expiry);
                
                _logger.LogDebug("Set expiry: {Key}, Expiry: {Expiry}, Success: {Success}", 
                    prefixedKey, expiry, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set expiry: {Key}", key);
                return false;
            }
        }

        public virtual async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var ttl = await connection.GetTimeToLiveAsync(prefixedKey);
                
                _logger.LogDebug("Get TTL: {Key}, TTL: {TTL}", prefixedKey, ttl);
                return ttl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get TTL: {Key}", key);
                return null;
            }
        }

        public virtual async Task<bool> RemoveExpiryAsync(string key)
        {
            try
            {
                var connection = await GetConnectionAsync();
                var prefixedKey = GetPrefixedKey(key);
                var result = await connection.RemoveExpiryAsync(prefixedKey);
                
                _logger.LogDebug("Remove expiry: {Key}, Success: {Success}", prefixedKey, result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove expiry: {Key}", key);
                return false;
            }
        }

        #endregion

        #region Cache Operations

        public virtual async Task<T?> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
        {
            try
            {
                // Try to get from cache first
                var cachedEntity = await GetAsync(key);
                if (cachedEntity != null)
                {
                    _logger.LogDebug("Cache hit for key: {Key}", key);
                    return cachedEntity;
                }

                // Cache miss, get from factory
                _logger.LogDebug("Cache miss for key: {Key}, calling factory", key);
                var entity = await factory();
                
                if (entity != null)
                {
                    await SetAsync(key, entity, expiry);
                    _logger.LogDebug("Cached entity for key: {Key}", key);
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed in GetOrSet for key: {Key}", key);
                return null;
            }
        }

        public virtual async Task<bool> RefreshAsync(string key, TimeSpan? expiry = null)
        {
            try
            {
                var entity = await GetAsync(key);
                if (entity == null)
                {
                    return false;
                }

                return await SetAsync(key, entity, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh key: {Key}", key);
                return false;
            }
        }

        #endregion

        #region Utility Operations

        public virtual async Task<bool> FlushAllAsync()
        {
            try
            {
                var connection = await GetConnectionAsync();
                var result = await connection.FlushAllAsync();
                
                _logger.LogWarning("Database flushed - all data deleted");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to flush database");
                return false;
            }
        }

        public virtual async Task<long> GetDatabaseSizeAsync()
        {
            try
            {
                var connection = await GetConnectionAsync();
                var size = await connection.GetDatabaseSizeAsync();
                
                _logger.LogDebug("Database size: {Size} keys", size);
                return size;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get database size");
                return -1;
            }
        }

        public virtual async Task<bool> TestConnectionAsync()
        {
            try
            {
                var connection = await GetConnectionAsync();
                var result = await connection.TestConnectionAsync();
                
                _logger.LogDebug("Connection test successful: {Result}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Connection test failed");
                return false;
            }
        }

        #endregion

        #region Dispose

        public virtual void Dispose()
        {
            try
            {
                _connection?.Dispose();
                _logger.LogDebug("Object storage repository disposed for {EntityType}", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing object storage repository");
            }
        }

        #endregion
    }
}