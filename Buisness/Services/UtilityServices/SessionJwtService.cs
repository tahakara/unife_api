using Buisness.Abstract.ServicesBase;
using Core.Security.JWT.Abstractions;
using Core.Security.JWT.Extensions;
using Core.ObjectStorage.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;
using Buisness.Services.UtilityServices.Abtract;

namespace Buisness.Services.UtilityServices
{

    /// <summary>
    /// Result of token refresh operation
    /// </summary>
    public class RefreshTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string UserUuid { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime RefreshExpiresAt { get; set; }

    }

    public class SessionJwtService : ISessionJwtService
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SessionJwtService> _logger;

        public SessionJwtService(
            IJwtTokenProvider jwtTokenProvider,
            IServiceProvider serviceProvider,
            ILogger<SessionJwtService> logger)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        private async Task<IObjectStorageConnection> GetSessionConnectionAsync()
        {
            var sessionFactory = _serviceProvider.GetRequiredKeyedService<IObjectStorageConnectionFactory>("session");
            return await sessionFactory.CreateConnectionAsync();
        }

        private string GetAccessTokenKey(string sessionUuid, string userUuid)
        {
            return $"access:{sessionUuid}:{userUuid}";
        }

        private string GetRefreshTokenKey(string sessionUuid, string userUuid, string refreshToken)
        {
            return $"refresh:{sessionUuid}:{userUuid}:{refreshToken}";
        }

        public async Task<string> GenerateAccessTokenAsync(string userUuid, string sessionUuid, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = JwtExtensions.CreateUserSessionClaims(userUuid, sessionUuid, additionalClaims);
            var accessToken = _jwtTokenProvider.GenerateAccessToken(claims);

            // Store access token with new key structure
            try
            {
                using var connection = await GetSessionConnectionAsync();
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);

                var tokenData = new
                {
                    Token = accessToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15) // Default expiration
                };

                var serializedData = JsonSerializer.Serialize(tokenData);
                await connection.SetStringAsync(accessKey, serializedData, TimeSpan.FromMinutes(15));

                _logger.LogDebug("Access token generated and stored for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store access token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }

            return accessToken;
        }

        public async Task<string> GenerateRefreshTokenAsync(string userUuid, string sessionUuid)
        {
            var refreshToken = _jwtTokenProvider.GenerateRefreshToken();

            // Store refresh token with new key structure
            try
            {
                using var connection = await GetSessionConnectionAsync();
                var refreshKey = GetRefreshTokenKey(sessionUuid, userUuid, refreshToken);

                var tokenData = new
                {
                    Token = refreshToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7) // Default expiration
                };

                var serializedData = JsonSerializer.Serialize(tokenData);
                await connection.SetStringAsync(refreshKey, serializedData, TimeSpan.FromDays(7));

                _logger.LogDebug("Refresh token generated and stored for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to store refresh token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }

            return refreshToken;
        }

        public async Task<bool> ValidateAndStoreTokenAsync(string userUuid, string sessionUuid, string accessToken, string refreshToken)
        {
            try
            {
                // Validate access token structure
                if (!_jwtTokenProvider.ValidateToken(accessToken, out var principal))
                {
                    _logger.LogWarning("Invalid access token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                    return false;
                }

                // Verify token belongs to the correct user and session
                var tokenUserUuid = principal?.GetUserUuid();
                var tokenSessionUuid = principal?.GetSessionUuid();

                if (tokenUserUuid != userUuid || tokenSessionUuid != sessionUuid)
                {
                    _logger.LogWarning("Token user/session mismatch for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                    return false;
                }

                // Store tokens with new key structure
                using var connection = await GetSessionConnectionAsync();

                // Store access token
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);
                var accessTokenData = new
                {
                    Token = accessToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15)
                };
                await connection.SetStringAsync(accessKey, JsonSerializer.Serialize(accessTokenData), TimeSpan.FromMinutes(15));

                // Store refresh token
                var refreshKey = GetRefreshTokenKey(userUuid, sessionUuid, refreshToken);
                var refreshTokenData = new
                {
                    Token = refreshToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };
                await connection.SetStringAsync(refreshKey, JsonSerializer.Serialize(refreshTokenData), TimeSpan.FromDays(7));

                _logger.LogDebug("Tokens validated and stored for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating and storing tokens for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return false;
            }
        }

        public async Task<string?> GetRefreshTokenKeyByRefreshTokenPostfixAsync(string refreshTokenPostfix)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                var pattern = $"refresh:*:*:{refreshTokenPostfix}";
                var keys = await connection.GetKeysAsync(pattern);
                if (keys.Count == 0)
                {
                    _logger.LogInformation("No refresh token found with postfix: {RefreshTokenPostfix}", refreshTokenPostfix);
                    return null;
                }
                else if (keys.Count > 1)
                {
                    _logger.LogWarning("Multiple refresh tokens found with postfix: {RefreshTokenPostfix}. Returning first match.", refreshTokenPostfix);
                    return null; // or handle as needed
                }

                _logger.LogInformation("Found refresh token key for postfix: {RefreshTokenPostfix}", refreshTokenPostfix);
                return keys.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving refresh token key for postfix: {RefreshTokenPostfix}", refreshTokenPostfix);
                return null;
            }
        }

        public async Task<RefreshTokenResult?> RefreshAccessTokenAsync(string refreshToken)
        {
            try
            {
                var key = await GetRefreshTokenKeyByRefreshTokenPostfixAsync(refreshToken);
                if (string.IsNullOrEmpty(key))
                {
                    _logger.LogWarning("Refresh token key not found for token: {RefreshToken}", refreshToken);
                    return null;
                }

                var parts = key.Split(':');
                var userUuid = string.Empty;
                var sessionUuid = string.Empty;
                // Defensive: Ensure at least 4 parts
                if (parts.Length < 4)
                {
                    return null;
                }
                // Last part is refreshToken, second last is userUuid, third last is sessionUuid
                refreshToken = parts[^1];
                userUuid = parts[^2];
                sessionUuid = parts[^3];

                return await RefreshAccessTokenAsync(userUuid, sessionUuid, refreshToken);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing access token with refresh token: {RefreshToken}", refreshToken);
                return null;
            }
        }


        public async Task<RefreshTokenResult?> RefreshAccessTokenAsync(string userUuid, string sessionUuid, string refreshToken)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                var refreshKey = GetRefreshTokenKey(sessionUuid, userUuid, refreshToken);

                // Get refresh token data
                var refreshTokenJson = await connection.GetStringAsync(refreshKey);
                if (string.IsNullOrEmpty(refreshTokenJson))
                {
                    _logger.LogWarning("Refresh token not found: {RefreshToken}", refreshToken);
                    return null;
                }

                using var jsonDoc = JsonDocument.Parse(refreshTokenJson);
                var root = jsonDoc.RootElement;

                var storedUserUuid = root.GetProperty("UserUuid").GetString();
                var storedSessionUuid = root.GetProperty("SessionUuid").GetString();

                if (string.IsNullOrEmpty(storedUserUuid) || string.IsNullOrEmpty(storedSessionUuid))
                {
                    _logger.LogWarning("Invalid refresh token data: {RefreshToken}", refreshToken);
                    return null;
                }
                
                // Remove old refresh token
                await connection.DeleteAsync(refreshKey);
                
                // Remove old access token
                var oldAccessKey = GetAccessTokenKey(storedSessionUuid, storedUserUuid);
                await connection.DeleteAsync(oldAccessKey);                

                // Generate new access token
                var claims = JwtExtensions.CreateUserSessionClaims(storedUserUuid, storedSessionUuid);
                var newAccessToken = _jwtTokenProvider.GenerateAccessToken(claims);

                // Store new access token using existing connection
                var accessKey = GetAccessTokenKey(storedSessionUuid, storedUserUuid);
                var accessTokenData = new
                {
                    Token = newAccessToken,
                    UserUuid = storedUserUuid,
                    SessionUuid = storedSessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15)
                };
                var serializedData = JsonSerializer.Serialize(accessTokenData);
                await connection.SetStringAsync(accessKey, serializedData, TimeSpan.FromMinutes(15));

                // Generate new refresh token using existing connection
                var newRefreshToken = _jwtTokenProvider.GenerateRefreshToken();
                var newRefreshKey = GetRefreshTokenKey(storedSessionUuid, storedUserUuid, newRefreshToken);
                var refreshTokenData = new
                {
                    Token = newRefreshToken,
                    UserUuid = storedUserUuid,
                    SessionUuid = storedSessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };
                var refreshSerializedData = JsonSerializer.Serialize(refreshTokenData);
                await connection.SetStringAsync(newRefreshKey, refreshSerializedData, TimeSpan.FromDays(7));

                _logger.LogInformation("Tokens refreshed for user: {UserUuid}, session: {SessionUuid}", storedUserUuid, sessionUuid);

                return new RefreshTokenResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserUuid = storedUserUuid,
                    SessionUuid = sessionUuid,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    RefreshExpiresAt = DateTime.UtcNow.AddDays(7)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing tokens with refresh token: {RefreshToken}", refreshToken);
                return null;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (!_jwtTokenProvider.ValidateToken(token, out var principal))
            {
                return false;
            }

            var userUuid = principal?.GetUserUuid();
            var sessionUuid = principal?.GetSessionUuid();

            if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid))
            {
                return false;
            }

            try
            {
                using var connection = await GetSessionConnectionAsync();
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);

                var storedTokenJson = await connection.GetStringAsync(accessKey);
                if (string.IsNullOrEmpty(storedTokenJson))
                {
                    _logger.LogWarning("Access token not found in storage for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                    return false;
                }

                using var jsonDoc = JsonDocument.Parse(storedTokenJson);
                var storedToken = jsonDoc.RootElement.GetProperty("Token").GetString();

                if (storedToken != token)
                {
                    _logger.LogWarning("Token mismatch for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return false;
            }
        }

        public async Task<bool> RevokeTokensAsync(string userUuid, string sessionUuid)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();

                // Remove refresh tokens for this user/session
                var refreshPattern = $"refresh:{sessionUuid}:{userUuid}:*";
                var refreshKeys = await connection.GetKeysAsync(refreshPattern);

                var keysToDelete = new List<string>();
                foreach (var key in refreshKeys)
                {
                    var tokenJson = await connection.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(tokenJson))
                    {
                        try
                        {
                            using var jsonDoc = JsonDocument.Parse(tokenJson);
                            var root = jsonDoc.RootElement;
                            var tokenUserUuid = root.GetProperty("UserUuid").GetString();
                            var tokenSessionUuid = root.GetProperty("SessionUuid").GetString();

                            if (tokenUserUuid == userUuid && tokenSessionUuid == sessionUuid)
                            {
                                keysToDelete.Add(key);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error parsing refresh token JSON for key: {Key}", key);
                        }
                    }
                }

                if (keysToDelete.Any())
                {
                    await connection.DeleteBatchAsync(keysToDelete);
                }

                // Remove access token
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);
                await connection.DeleteAsync(accessKey);

                _logger.LogInformation("Tokens revoked for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking tokens for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                return false;
            }
        }

        public async Task<bool> RevokeUserAllTokensAsync(string userUuid)
        {
            try
            {
                _logger.LogInformation("Revoking all tokens for user: {UserUuid}", userUuid);
                using var connection = await GetSessionConnectionAsync();

                // Remove all access tokens for user
                var accessPattern = $"access:*:{userUuid}";
                var accessKeys = await connection.GetKeysAsync(accessPattern);
                var accessKeysToDelete = new List<string>();
                foreach (var key in accessKeys)
                {
                    accessKeysToDelete.Add(key);
                }

                // Remove all refresh tokens for user
                var refreshPattern = $"refresh:{userUuid}:*:*";
                var refreshKeys = await connection.GetKeysAsync(refreshPattern);
                var refreshKeysToDelete = new List<string>();
                foreach (var key in refreshKeys)
                {
                    refreshKeysToDelete.Add(key);
                }
                var allKeysToDelete = accessKeysToDelete.Concat(refreshKeysToDelete).ToList();
                if (allKeysToDelete.Any())
                {
                    await connection.DeleteBatchAsync(allKeysToDelete);
                }
                _logger.LogInformation("All tokens revoked for user: {UserUuid}, Keys: {Count}", userUuid, allKeysToDelete.Count);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking all tokens for user: {UserUuid}", userUuid);
                return false;
            }
        }

        public async Task<bool> RevokeUserAllTokensExcluededByOne(string userUuid, string excluededSessionUuid)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                // Remove all access tokens for user except the excluded session
                var accessPattern = $"access:*:{userUuid}";
                var accessKeys = await connection.GetKeysAsync(accessPattern);
                var accessKeysToDelete = new List<string>();
                foreach (var key in accessKeys)
                {
                    if (!key.Contains(excluededSessionUuid))
                    {
                        accessKeysToDelete.Add(key);
                    }
                }
                // Remove all refresh tokens for user except the excluded session
                var refreshPattern = $"refresh:*:{userUuid}";
                var refreshKeys = await connection.GetKeysAsync(refreshPattern);
                var refreshKeysToDelete = new List<string>();
                foreach (var key in refreshKeys)
                {
                    if (!key.Contains(excluededSessionUuid))
                    {
                        refreshKeysToDelete.Add(key);
                    }
                }
                var allKeysToDelete = accessKeysToDelete.Concat(refreshKeysToDelete).ToList();
                if (allKeysToDelete.Any())
                {
                    await connection.DeleteBatchAsync(allKeysToDelete);
                }
                _logger.LogInformation("All tokens revoked for user: {UserUuid}, excluding session: {ExcludedSessionUuid}, Keys: {Count}", userUuid, excluededSessionUuid, allKeysToDelete.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking all tokens for user: {UserUuid}, excluding session: {ExcludedSessionUuid}", userUuid, excluededSessionUuid);
                return false;
            }
        }
        public string? GetUserUuidFromToken(string token)
        {
            if (_jwtTokenProvider.ValidateToken(token, out var principal))
            {
                return principal?.GetUserUuid();
            }
            return null;
        }

        public string? GetSessionUuidFromToken(string token)
        {
            if (_jwtTokenProvider.ValidateToken(token, out var principal))
            {
                return principal?.GetSessionUuid();
            }
            return null;
        }

        public async Task<string?> GetUserUuidFromTokenAsync(string token)
        {
            if (_jwtTokenProvider.ValidateToken(token, out var principal))
            {
                return await Task.FromResult(principal?.GetUserUuid());
            }
            return await Task.FromResult<string?>(null);
        }

        public async Task<string?> GetSessionUuidFromTokenAsync(string token)
        {
            if (_jwtTokenProvider.ValidateToken(token, out var principal))
            {
                return await Task.FromResult(principal?.GetSessionUuid());
            }
            return await Task.FromResult<string?>(null);
        }

        public async Task<JsonElement?> GetRefreshTokenValue(string refreshTokenKey)
        {
            try
            {
                _logger.LogDebug("Retrieving refresh token value for key: {RefreshTokenKey}", refreshTokenKey);
                using var connection = await GetSessionConnectionAsync();
                var refreshTokenJson = await connection.GetStringAsync(refreshTokenKey);
                if (string.IsNullOrEmpty(refreshTokenJson))
                {
                    _logger.LogWarning("Refresh token not found for key: {RefreshTokenKey}", refreshTokenKey);
                    return null;
                }
                var jsonDoc = JsonDocument.Parse(refreshTokenJson);

                return jsonDoc.RootElement; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving refresh token value for key: {RefreshTokenKey}", refreshTokenKey);
                return null;

            }
        }
    }
}