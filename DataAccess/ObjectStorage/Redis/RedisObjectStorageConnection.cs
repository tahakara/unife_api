using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.ObjectStorage.Redis
{
    public class RedisObjectStorageConnection : IObjectStorageConnection
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly string _containerName;
        private readonly RedisStorageType _storageType;
        private readonly string _keyPrefix;

        public RedisObjectStorageConnection(
            IConnectionMultiplexer connectionMultiplexer,
            IDatabase database,
            string containerName,
            RedisStorageType storageType = RedisStorageType.Cache)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
            _storageType = storageType;
            _keyPrefix = RedisStorageConfiguration.GetKeyPrefix(storageType);
        }

        public bool IsConnected => _connectionMultiplexer.IsConnected;
        public string ContainerName => _containerName;
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public RedisStorageType StorageType => _storageType;

        /// <summary>
        /// Gets the prefixed key with storage type prefix
        /// </summary>
        /// <param name="key">Original key</param>
        /// <returns>Prefixed key</returns>
        private string GetPrefixedKey(string key)
        {
            return $"{_keyPrefix}:{key}";
        }

        // Basic operations
        public async Task<string?> GetStringAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            var value = await _database.StringGetAsync(prefixedKey);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            var prefixedKey = GetPrefixedKey(key);
            var defaultExpiry = expiry ?? RedisStorageConfiguration.GetDefaultExpiration(_storageType);
            return await _database.StringSetAsync(prefixedKey, value, defaultExpiry);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.KeyDeleteAsync(prefixedKey);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.KeyExistsAsync(prefixedKey);
        }

        // Hash operations
        public async Task<bool> SetHashFieldAsync(string key, string field, string value)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.HashSetAsync(prefixedKey, field, value);
        }

        public async Task<string?> GetHashFieldAsync(string key, string field)
        {
            var prefixedKey = GetPrefixedKey(key);
            var value = await _database.HashGetAsync(prefixedKey, field);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<Dictionary<string, string>> GetAllHashFieldsAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            var hashEntries = await _database.HashGetAllAsync(prefixedKey);
            return hashEntries.ToDictionary(
                entry => entry.Name.ToString(),
                entry => entry.Value.ToString()
            );
        }

        public async Task<bool> DeleteHashFieldAsync(string key, string field)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.HashDeleteAsync(prefixedKey, field);
        }

        // List operations
        public async Task<long> AddToListAsync(string key, string value, bool leftPush = true)
        {
            var prefixedKey = GetPrefixedKey(key);
            return leftPush
                ? await _database.ListLeftPushAsync(prefixedKey, value)
                : await _database.ListRightPushAsync(prefixedKey, value);
        }

        public async Task<string?> PopFromListAsync(string key, bool leftPop = true)
        {
            var prefixedKey = GetPrefixedKey(key);
            var result = leftPop
                ? await _database.ListLeftPopAsync(prefixedKey)
                : await _database.ListRightPopAsync(prefixedKey);
            return result.HasValue ? result.ToString() : null;
        }

        public async Task<List<string>> GetListRangeAsync(string key, long start = 0, long stop = -1)
        {
            var prefixedKey = GetPrefixedKey(key);
            var values = await _database.ListRangeAsync(prefixedKey, start, stop);
            return values.Select(v => v.ToString()).ToList();
        }

        public async Task<long> GetListLengthAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.ListLengthAsync(prefixedKey);
        }

        // Set operations
        public async Task<bool> AddToSetAsync(string key, string value)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.SetAddAsync(prefixedKey, value);
        }

        public async Task<bool> RemoveFromSetAsync(string key, string value)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.SetRemoveAsync(prefixedKey, value);
        }

        public async Task<bool> IsInSetAsync(string key, string value)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.SetContainsAsync(prefixedKey, value);
        }

        public async Task<List<string>> GetSetMembersAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            var values = await _database.SetMembersAsync(prefixedKey);
            return values.Select(v => v.ToString()).ToList();
        }

        public async Task<long> GetSetCountAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.SetLengthAsync(prefixedKey);
        }

        // Batch operations
        public async Task<Dictionary<string, string?>> GetBatchAsync(IEnumerable<string> keys)
        {
            var prefixedKeys = keys.Select(GetPrefixedKey).ToArray();
            var redisKeys = prefixedKeys.Select(k => (RedisKey)k).ToArray();
            var values = await _database.StringGetAsync(redisKeys);

            var result = new Dictionary<string, string?>();
            for (int i = 0; i < keys.Count(); i++)
            {
                var originalKey = keys.ElementAt(i);
                result[originalKey] = values[i].HasValue ? values[i].ToString() : null;
            }

            return result;
        }

        public async Task<bool> SetBatchAsync(Dictionary<string, string> keyValuePairs, TimeSpan? expiry = null)
        {
            var prefixedPairs = keyValuePairs.Select(kvp =>
                new KeyValuePair<RedisKey, RedisValue>(GetPrefixedKey(kvp.Key), kvp.Value)).ToArray();

            var result = await _database.StringSetAsync(prefixedPairs);

            if (result && expiry.HasValue)
            {
                foreach (var kvp in keyValuePairs)
                {
                    await _database.KeyExpireAsync(GetPrefixedKey(kvp.Key), expiry.Value);
                }
            }
            else if (result)
            {
                var defaultExpiry = RedisStorageConfiguration.GetDefaultExpiration(_storageType);
                foreach (var kvp in keyValuePairs)
                {
                    await _database.KeyExpireAsync(GetPrefixedKey(kvp.Key), defaultExpiry);
                }
            }

            return result;
        }

        public async Task<long> DeleteBatchAsync(IEnumerable<string> keys)
        {
            var prefixedKeys = keys.Select(GetPrefixedKey);
            var redisKeys = prefixedKeys.Select(k => (RedisKey)k).ToArray();
            return await _database.KeyDeleteAsync(redisKeys);
        }

        // Pattern operations
        public async Task<List<string>> GetKeysAsync(string pattern = "*")
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var prefixedPattern = GetPrefixedKey(pattern);
            var keys = server.Keys(database: _database.Database, pattern: prefixedPattern)
                .Select(k => k.ToString().Replace($"{_keyPrefix}:", ""))
                .ToList();
            return keys;
        }

        // Expiry operations
        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.KeyExpireAsync(prefixedKey, expiry);
        }

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.KeyTimeToLiveAsync(prefixedKey);
        }

        public async Task<bool> RemoveExpiryAsync(string key)
        {
            var prefixedKey = GetPrefixedKey(key);
            return await _database.KeyPersistAsync(prefixedKey);
        }

        // Utility operations
        public async Task<bool> FlushAllAsync()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            await server.FlushDatabaseAsync(_database.Database);
            return true;
        }

        public async Task<long> GetDatabaseSizeAsync()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            return await server.DatabaseSizeAsync(_database.Database);
        }

        public async Task<bool> TestConnectionAsync()
        {
            var pingResult = await _database.PingAsync();
            return pingResult.TotalMilliseconds > 0;
        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
        }
    }
}