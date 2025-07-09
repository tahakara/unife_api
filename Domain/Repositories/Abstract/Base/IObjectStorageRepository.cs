using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.Abstract.Base
{
    public interface IObjectStorageRepository<T> where T : class
    {
        // Basic CRUD operations
        Task<T?> GetAsync(string key);
        Task<List<T>> GetAllAsync(string pattern = "*");
        Task<bool> SetAsync(string key, T entity, TimeSpan? expiry = null);
        Task<bool> UpdateAsync(string key, T entity, TimeSpan? expiry = null);
        Task<bool> DeleteAsync(string key);
        Task<bool> ExistsAsync(string key);

        // JSON operations for complex objects
        Task<bool> SetJsonAsync(string key, T entity, TimeSpan? expiry = null);
        Task<T?> GetJsonAsync(string key);

        // Hash operations for entity properties
        Task<bool> SetHashFieldAsync(string key, string field, string value);
        Task<string?> GetHashFieldAsync(string key, string field);
        Task<Dictionary<string, string>> GetAllHashFieldsAsync(string key);
        Task<bool> DeleteHashFieldAsync(string key, string field);
        Task<bool> SetEntityAsHashAsync(string key, T entity, TimeSpan? expiry = null);
        Task<T?> GetEntityFromHashAsync(string key);

        // List operations for collections
        Task<long> AddToListAsync(string key, T entity, bool leftPush = true);
        Task<T?> PopFromListAsync(string key, bool leftPop = true);
        Task<List<T>> GetListRangeAsync(string key, long start = 0, long stop = -1);
        Task<long> GetListLengthAsync(string key);

        // Set operations for unique collections
        Task<bool> AddToSetAsync(string key, T entity);
        Task<bool> RemoveFromSetAsync(string key, T entity);
        Task<bool> IsInSetAsync(string key, T entity);
        Task<List<T>> GetSetMembersAsync(string key);
        Task<long> GetSetCountAsync(string key);

        // Batch operations
        Task<Dictionary<string, T?>> GetBatchAsync(IEnumerable<string> keys);
        Task<bool> SetBatchAsync(Dictionary<string, T> keyValuePairs, TimeSpan? expiry = null);
        Task<long> DeleteBatchAsync(IEnumerable<string> keys);

        // Search and pattern operations
        Task<List<string>> GetKeysAsync(string pattern = "*");
        Task<List<T>> GetByPatternAsync(string pattern = "*");
        Task<long> CountByPatternAsync(string pattern = "*");

        // Expiry operations
        Task<bool> SetExpiryAsync(string key, TimeSpan expiry);
        Task<TimeSpan?> GetTimeToLiveAsync(string key);
        Task<bool> RemoveExpiryAsync(string key);

        // Cache-specific operations
        Task<T?> GetOrSetAsync(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
        Task<bool> RefreshAsync(string key, TimeSpan? expiry = null);

        // Utility operations
        Task<bool> FlushAllAsync();
        Task<long> GetDatabaseSizeAsync();
        Task<bool> TestConnectionAsync();
    }
}