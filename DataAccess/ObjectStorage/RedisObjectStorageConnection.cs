using Core.ObjectStorage.Base;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.ObjectStorage
{
    public class RedisObjectStorageConnection : IObjectStorageConnection
    // Ensure there is only one definition of RedisObjectStorageConnection in the namespace  
    // Check for duplicate class definitions in the same namespace  
    // If there is another file with the same class, consider renaming or removing the duplicate
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly string _containerName;

        public RedisObjectStorageConnection(
            IConnectionMultiplexer connectionMultiplexer,
            IDatabase database,
            string containerName)
        {
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
        }

        public bool IsConnected => _connectionMultiplexer.IsConnected;
        public string ContainerName => _containerName;
        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        // Basic operations
        public async Task<string?> GetStringAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _database.StringSetAsync(key, value, expiry);
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        // Hash operations
        public async Task<bool> SetHashFieldAsync(string key, string field, string value)
        {
            return await _database.HashSetAsync(key, field, value);
        }

        public async Task<string?> GetHashFieldAsync(string key, string field)
        {
            var value = await _database.HashGetAsync(key, field);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task<Dictionary<string, string>> GetAllHashFieldsAsync(string key)
        {
            var hashEntries = await _database.HashGetAllAsync(key);
            return hashEntries.ToDictionary(
                entry => entry.Name.ToString(),
                entry => entry.Value.ToString()
            );
        }

        public async Task<bool> DeleteHashFieldAsync(string key, string field)
        {
            return await _database.HashDeleteAsync(key, field);
        }

        // List operations
        public async Task<long> AddToListAsync(string key, string value, bool leftPush = true)
        {
            return leftPush
                ? await _database.ListLeftPushAsync(key, value)
                : await _database.ListRightPushAsync(key, value);
        }

        public async Task<string?> PopFromListAsync(string key, bool leftPop = true)
        {
            var result = leftPop
                ? await _database.ListLeftPopAsync(key)
                : await _database.ListRightPopAsync(key);
            return result.HasValue ? result.ToString() : null;
        }

        public async Task<List<string>> GetListRangeAsync(string key, long start = 0, long stop = -1)
        {
            var values = await _database.ListRangeAsync(key, start, stop);
            return values.Select(v => v.ToString()).ToList();
        }

        public async Task<long> GetListLengthAsync(string key)
        {
            return await _database.ListLengthAsync(key);
        }

        // Set operations
        public async Task<bool> AddToSetAsync(string key, string value)
        {
            return await _database.SetAddAsync(key, value);
        }

        public async Task<bool> RemoveFromSetAsync(string key, string value)
        {
            return await _database.SetRemoveAsync(key, value);
        }

        public async Task<bool> IsInSetAsync(string key, string value)
        {
            return await _database.SetContainsAsync(key, value);
        }

        public async Task<List<string>> GetSetMembersAsync(string key)
        {
            var values = await _database.SetMembersAsync(key);
            return values.Select(v => v.ToString()).ToList();
        }

        public async Task<long> GetSetCountAsync(string key)
        {
            return await _database.SetLengthAsync(key);
        }

        // Batch operations
        public async Task<Dictionary<string, string?>> GetBatchAsync(IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
            var values = await _database.StringGetAsync(redisKeys);

            var result = new Dictionary<string, string?>();
            for (int i = 0; i < keys.Count(); i++)
            {
                var key = keys.ElementAt(i);
                result[key] = values[i].HasValue ? values[i].ToString() : null;
            }

            return result;
        }

        public async Task<bool> SetBatchAsync(Dictionary<string, string> keyValuePairs, TimeSpan? expiry = null)
        {
            var keyValues = keyValuePairs.Select(kvp =>
                new KeyValuePair<RedisKey, RedisValue>(kvp.Key, kvp.Value)).ToArray();

            var result = await _database.StringSetAsync(keyValues);

            if (result && expiry.HasValue)
            {
                foreach (var kvp in keyValuePairs)
                {
                    await _database.KeyExpireAsync(kvp.Key, expiry.Value);
                }
            }

            return result;
        }

        public async Task<long> DeleteBatchAsync(IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
            return await _database.KeyDeleteAsync(redisKeys);
        }

        // Pattern operations
        public async Task<List<string>> GetKeysAsync(string pattern = "*")
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern).Select(k => k.ToString()).ToList();
            return keys;
        }

        // Expiry operations
        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            return await _database.KeyExpireAsync(key, expiry);
        }

        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            return await _database.KeyTimeToLiveAsync(key);
        }

        public async Task<bool> RemoveExpiryAsync(string key)
        {
            return await _database.KeyPersistAsync(key);
        }

        // Utility operations
        public async Task<bool> FlushAllAsync()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            await server.FlushDatabaseAsync();
            return true;
        }

        public async Task<long> GetDatabaseSizeAsync()
        {
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            return await server.DatabaseSizeAsync();
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