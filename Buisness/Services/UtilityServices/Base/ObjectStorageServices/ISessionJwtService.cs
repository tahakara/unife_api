using Buisness.Services.UtilityServices.ObjectStorageServices;
using Core.Enums;
using System.Security.Claims;
using System.Text.Json;

namespace Buisness.Services.UtilityServices.Base.ObjectStorageServices
{
    /// <summary>
    /// Session-aware JWT service that integrates with session management
    /// </summary>
    public interface ISessionJwtService
    {
        Task<string> GenerateAccessTokenAsync(string userTypeId, string userUuid, string sessionUuid, IEnumerable<Claim>? additionalClaims = null);
        //Task<string> GenerateAccessTokenAsync(string userUuid, string sessionUuid, IEnumerable<Claim>? additionalClaims = null);
        Task<string> GenerateRefreshTokenAsync(string userTypeId, string userUuid, string sessionUuid);
        //Task<string> GenerateRefreshTokenAsync(string userUuid, string sessionUuid);
        Task<bool> ValidateAndStoreTokenAsync(string userTypeId, string userUuid, string sessionUuid, string accessToken, string refreshToken);
        Task<string?> GetRefreshTokenKeyByRefreshTokenPostfixAsync(string refreshTokenPostfix);
        Task<JsonElement?> GetRefreshTokenValue(string refreshTokenKey);
        Task<RefreshTokenResult?> RefreshAccessTokenAsync(string refreshToken);
        Task<RefreshTokenResult?> RefreshAccessTokenAsync(string userTypeId, string userUuid, string sessionUuid, string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> RevokeTokensAsync(string userUuid, string sessionUuid, string userTypeId);
        Task<bool> RevokeUserAllTokensAsync(string userUuid);
        Task<bool> RevokeUserAllTokensExcluededByOne(string userUuid, string excluededSessionUuid);
        string? GetUserUuidFromToken(string token);
        string? GetSessionUuidFromToken(string token);

        Task<string?> GetUserUuidFromTokenAsync(string token);
        Task<string?> GetSessionUuidFromTokenAsync(string token);
        Task<string?> GetUserTypeIdFromTokenAsync(string token);

        Task<int> GetSessionCountByUserUuid(string useUuid);

        #region Brute Force Protection
        string GenerateBruteForceProtectionKey(string? email, string? phoneCode, string? phoneNumber);
        Task<bool> IsBruteForceProtectionKeyExistsAsync(string key);
        Task<bool> RemoveBruteForceProtectionKeyAsync(string key);
        Task<bool> SetBruteForceProtectionKeyAsync(string key, int attempts, TimeSpan? expiration = null);
        Task<int> GetBruteForceProtectionAttemptsByKeyAsync(string key);
        Task<bool> IncrementBruteForceProtectionAttemptsAsync(string key);
        Task<bool> ResetBruteForceProtectionAttemptsAsync(string key);
        #endregion
    }
}