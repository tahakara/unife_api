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

        private string GetRefreshTokenKey(string refreshToken)
        {
            return $"refresh:{refreshToken}";
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
                var refreshKey = GetRefreshTokenKey(refreshToken);
                
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
                var refreshKey = GetRefreshTokenKey(refreshToken);
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

        public async Task<RefreshTokenResult?> RefreshAccessTokenAsync(string refreshToken)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                var refreshKey = GetRefreshTokenKey(refreshToken);
                
                // Get refresh token data
                var refreshTokenJson = await connection.GetStringAsync(refreshKey);
                if (string.IsNullOrEmpty(refreshTokenJson))
                {
                    _logger.LogWarning("Refresh token not found: {RefreshToken}", refreshToken);
                    return null;
                }

                using var jsonDoc = JsonDocument.Parse(refreshTokenJson);
                var root = jsonDoc.RootElement;
                
                var userUuid = root.GetProperty("UserUuid").GetString();
                var sessionUuid = root.GetProperty("SessionUuid").GetString();

                if (string.IsNullOrEmpty(userUuid) || string.IsNullOrEmpty(sessionUuid))
                {
                    _logger.LogWarning("Invalid refresh token data: {RefreshToken}", refreshToken);
                    return null;
                }

                // Generate new access token using the existing connection
                var claims = JwtExtensions.CreateUserSessionClaims(userUuid, sessionUuid);
                var newAccessToken = _jwtTokenProvider.GenerateAccessToken(claims);

                // Store new access token using existing connection
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);
                var accessTokenData = new
                {
                    Token = newAccessToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15)
                };
                var serializedData = JsonSerializer.Serialize(accessTokenData);
                await connection.SetStringAsync(accessKey, serializedData, TimeSpan.FromMinutes(15));

                // Generate new refresh token using existing connection
                var newRefreshToken = _jwtTokenProvider.GenerateRefreshToken();
                var newRefreshKey = GetRefreshTokenKey(newRefreshToken);
                var refreshTokenData = new
                {
                    Token = newRefreshToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };
                var refreshSerializedData = JsonSerializer.Serialize(refreshTokenData);
                await connection.SetStringAsync(newRefreshKey, refreshSerializedData, TimeSpan.FromDays(7));

                // Remove old refresh token
                await connection.DeleteAsync(refreshKey);

                _logger.LogInformation("Tokens refreshed for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
                
                return new RefreshTokenResult
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserUuid = userUuid,
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

        public async Task RevokeTokensAsync(string userUuid, string sessionUuid)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                
                // Remove access token
                var accessKey = GetAccessTokenKey(sessionUuid, userUuid);
                await connection.DeleteAsync(accessKey);

                // Remove refresh tokens for this user/session
                var refreshPattern = $"refresh:*";
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

                _logger.LogInformation("Tokens revoked for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking tokens for user: {UserUuid}, session: {SessionUuid}", userUuid, sessionUuid);
            }
        }

        public async Task RevokeAllUserTokensAsync(string userUuid)
        {
            try
            {
                using var connection = await GetSessionConnectionAsync();
                
                // Remove all access tokens for user
                var accessPattern = $"access:*:{userUuid}";
                var accessKeys = await connection.GetKeysAsync(accessPattern);
                
                // Remove all refresh tokens for user
                var refreshPattern = $"refresh:*";
                var refreshKeys = await connection.GetKeysAsync(refreshPattern);
                
                var refreshKeysToDelete = new List<string>();
                foreach (var key in refreshKeys)
                {
                    var tokenJson = await connection.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(tokenJson))
                    {
                        try
                        {
                            using var jsonDoc = JsonDocument.Parse(tokenJson);
                            var tokenUserUuid = jsonDoc.RootElement.GetProperty("UserUuid").GetString();
                            
                            if (tokenUserUuid == userUuid)
                            {
                                refreshKeysToDelete.Add(key);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error parsing refresh token JSON for key: {Key}", key);
                        }
                    }
                }

                var allKeysToDelete = accessKeys.Concat(refreshKeysToDelete).ToList();
                if (allKeysToDelete.Any())
                {
                    await connection.DeleteBatchAsync(allKeysToDelete);
                }

                _logger.LogInformation("All tokens revoked for user: {UserUuid}, Keys: {Count}", userUuid, allKeysToDelete.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking all tokens for user: {UserUuid}", userUuid);
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
    }
}