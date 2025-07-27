using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.ObjectStorage.Base
{
    public interface IObjectStorageConnection : IDisposable
    {
        bool IsConnected { get; }
        string ContainerName { get; }
        DateTime CreatedAt { get; }

        // Basic operations
        Task<string?> GetStringAsync(string key);
        Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);

        // Hash operations
        Task<bool> SetHashFieldAsync(string key, string field, string value);
        Task<string?> GetHashFieldAsync(string key, string field);
        Task<Dictionary<string, string>> GetAllHashFieldsAsync(string key);
        Task<bool> DeleteHashFieldAsync(string key, string field);

        // List operations
        Task<long> AddToListAsync(string key, string value, bool leftPush = true);
        Task<string?> PopFromListAsync(string key, bool leftPop = true);
        Task<List<string>> GetListRangeAsync(string key, long start = 0, long stop = -1);
        Task<long> GetListLengthAsync(string key);

        // Set operations
        Task<bool> AddToSetAsync(string key, string value);
        Task<bool> RemoveFromSetAsync(string key, string value);
        Task<bool> IsInSetAsync(string key, string value);
        Task<List<string>> GetSetMembersAsync(string key);
        Task<long> GetSetCountAsync(string key);

        // Batch operations
        Task<Dictionary<string, string?>> GetBatchAsync(IEnumerable<string> keys);
        Task<bool> SetBatchAsync(Dictionary<string, string> keyValuePairs, TimeSpan? expiry = null);
        Task<long> DeleteBatchAsync(IEnumerable<string> keys);

        // Pattern operations
        Task<string> GetKeyByPatternAsync(string pattern);
        Task<List<string>> GetKeysAsync(string pattern = "*");

        // Expiry operations
        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
        Task<bool> RemoveExpiryAsync(string key);

        // Utility operations
        Task<bool> FlushAllAsync();
        Task<long> GetDatabaseSizeAsync();
        Task<bool> TestConnectionAsync();

        Task<long> IncrementStringAsync(IObjectStorageConnection conn, string key);
    }
}