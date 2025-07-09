namespace Buisness.Abstract.ServicesBase
{
    public interface ISessionService
    {
        Task<T?> GetAsync<T>(string userUuid, string sessionUuid, string key) where T : class;
        Task SetAsync<T>(string userUuid, string sessionUuid, string key, T value, TimeSpan? expiration = null) where T : class;
        Task RemoveAsync(string userUuid, string sessionUuid, string key);
        Task RemoveSessionAsync(string userUuid, string sessionUuid);
        Task RemoveAllUserSessionsAsync(string userUuid);
        Task<bool> SessionExistsAsync(string userUuid, string sessionUuid);
        Task RefreshSessionAsync(string userUuid, string sessionUuid, TimeSpan? expiration = null);
        Task<List<string>> GetUserSessionsAsync(string userUuid);
        
        // JWT Token management
        Task SetAccessTokenAsync(string userUuid, string sessionUuid, string token, TimeSpan? expiration = null);
        Task<string?> GetAccessTokenAsync(string userUuid, string sessionUuid);
        Task SetRefreshTokenAsync(string userUuid, string sessionUuid, string token, TimeSpan? expiration = null);
        Task<string?> GetRefreshTokenAsync(string userUuid, string sessionUuid);
        Task RevokeTokensAsync(string userUuid, string sessionUuid);
        Task RevokeAllUserTokensAsync(string userUuid);
    }
}