using Buisness.Abstract.ServicesBase;
using Buisness.Services.Base;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Buisness.Services.UtilityServices
{
    public class UnifeSessionService : UnifeStorageServiceBase, ISessionService
    {
        public UnifeSessionService(IServiceProvider serviceProvider, ILogger<UnifeSessionService> logger)
            : base(serviceProvider, logger, "session")
        {
        }

        private string GetSessionKey(string userUuid, string sessionUuid, string key)
        {
            return $"session:{userUuid}:{sessionUuid}:{key}";
        }

        private string GetSessionPattern(string userUuid, string sessionUuid)
        {
            return $"session:{userUuid}:{sessionUuid}:*";
        }

        private string GetUserSessionsPattern(string userUuid)
        {
            return $"session:{userUuid}:*";
        }

        public async Task<T?> GetAsync<T>(string userUuid, string sessionUuid, string key) where T : class
        {
            var sessionKey = GetSessionKey(userUuid, sessionUuid, key);
            return await GetAsync<T>(sessionKey);
        }

        public async Task SetAsync<T>(string userUuid, string sessionUuid, string key, T value, TimeSpan? expiration = null) where T : class
        {
            var sessionKey = GetSessionKey(userUuid, sessionUuid, key);
            await SetAsync(sessionKey, value, expiration);
        }

        public async Task RemoveAsync(string userUuid, string sessionUuid, string key)
        {
            var sessionKey = GetSessionKey(userUuid, sessionUuid, key);
            await RemoveAsync(sessionKey);
        }

        public async Task RemoveSessionAsync(string userUuid, string sessionUuid)
        {
            var pattern = GetSessionPattern(userUuid, sessionUuid);
            await RemoveByPatternAsync(pattern);
        }

        public async Task RemoveAllUserSessionsAsync(string userUuid)
        {
            var pattern = GetUserSessionsPattern(userUuid);
            await RemoveByPatternAsync(pattern);
        }

        public async Task<bool> SessionExistsAsync(string userUuid, string sessionUuid)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var pattern = GetSessionPattern(userUuid, sessionUuid);
                var keys = await connection.GetKeysAsync(pattern);
                return keys.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check session existence for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return false;
            }
        }

        public async Task RefreshSessionAsync(string userUuid, string sessionUuid, TimeSpan? expiration = null)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var pattern = GetSessionPattern(userUuid, sessionUuid);
                var keys = await connection.GetKeysAsync(pattern);
                
                var defaultExpiration = expiration ?? GetDefaultExpiration();
                
                foreach (var key in keys)
                {
                    await connection.SetExpiryAsync(key, defaultExpiration);
                }
                
                _logger.LogDebug("Session refreshed for user: {UserUuid}, session: {SessionUuid}, Keys: {Count}", userUuid, sessionUuid, keys.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh session for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }
        }

        public async Task<List<string>> GetUserSessionsAsync(string userUuid)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                var pattern = GetUserSessionsPattern(userUuid);
                var keys = await connection.GetKeysAsync(pattern);
                
                var sessionUuids = keys
                    .Select(key => key.Split(':'))
                    .Where(parts => parts.Length >= 3)
                    .Select(parts => parts[2])
                    .Distinct()
                    .ToList();
                
                return sessionUuids;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user sessions for user: {UserUuid}", userUuid);
                return new List<string>();
            }
        }

        // JWT Token management methods
        public async Task SetAccessTokenAsync(string userUuid, string sessionUuid, string token, TimeSpan? expiration = null)
        {
            await SetAsync(userUuid, sessionUuid, "access_token", new { Token = token, CreatedAt = DateTime.UtcNow }, expiration);
        }

        public async Task<string?> GetAccessTokenAsync(string userUuid, string sessionUuid)
        {
            try
            {
                var tokenData = await GetAsync<string>(userUuid, sessionUuid, "access_token");
                if (string.IsNullOrEmpty(tokenData)) return null;

                using var jsonDoc = JsonDocument.Parse(tokenData);
                return jsonDoc.RootElement.GetProperty("Token").GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get access token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return null;
            }
        }

        public async Task SetRefreshTokenAsync(string userUuid, string sessionUuid, string token, TimeSpan? expiration = null)
        {
            await SetAsync(userUuid, sessionUuid, "refresh_token", new { Token = token, CreatedAt = DateTime.UtcNow }, expiration);
        }

        public async Task<string?> GetRefreshTokenAsync(string userUuid, string sessionUuid)
        {
            try
            {
                var tokenData = await GetAsync<string>(userUuid, sessionUuid, "refresh_token");
                if (string.IsNullOrEmpty(tokenData)) return null;

                using var jsonDoc = JsonDocument.Parse(tokenData);
                return jsonDoc.RootElement.GetProperty("Token").GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get refresh token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return null;
            }
        }

        public async Task RevokeTokensAsync(string userUuid, string sessionUuid)
        {
            await RemoveAsync(userUuid, sessionUuid, "access_token");
            await RemoveAsync(userUuid, sessionUuid, "refresh_token");
            _logger.LogInformation("Tokens revoked for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
        }

        public async Task RevokeAllUserTokensAsync(string userUuid)
        {
            try
            {
                using var connection = await GetConnectionAsync();
                
                var accessTokenPattern = $"session:{userUuid}:*:access_token";
                var refreshTokenPattern = $"session:{userUuid}:*:refresh_token";
                
                var accessTokenKeys = await connection.GetKeysAsync(accessTokenPattern);
                var refreshTokenKeys = await connection.GetKeysAsync(refreshTokenPattern);
                
                var allTokenKeys = accessTokenKeys.Concat(refreshTokenKeys).ToList();
                
                if (allTokenKeys.Any())
                {
                    await connection.DeleteBatchAsync(allTokenKeys);
                    _logger.LogInformation("All tokens revoked for user: {UserUuid}, Keys: {Count}", userUuid, allTokenKeys.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke all tokens for user: {UserUuid}", userUuid);
            }
        }
    }
}