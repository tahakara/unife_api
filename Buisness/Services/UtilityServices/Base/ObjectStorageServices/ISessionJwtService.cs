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

        #region SignIn Brute Force Protection
        string GenerateSignInOTPBruteForceProtectionKey(string? email, string? phoneCode, string? phoneNumber);
        Task<bool> IsSignInOTPBruteForceProtectionKeyExistsAsync(string key);
        Task<bool> RemoveSignInOTPBruteForceProtectionKeyAsync(string key);
        //Task<bool> RevokeSignInOTPBruteForceProtectionByEmailAsync(string email);
        Task<bool> SetSignInOTPBruteForceProtectionKeyAsync(string key, int attempts = 0, TimeSpan? expiration = null);
        Task<int> GetSignInOTPBruteForceProtectionAttemptsByKeyAsync(string key);
        Task<int> GetSignInOTPBruteForceProtectionAttemptsByUserUuid(string userUuid);
        Task<bool> IncrementSignInOTPBruteForceProtectionAttemptsAsync(string key);
        Task<bool> ResetSignInOTPBruteForceProtectionAttemptsAsync(string key);
        #endregion

        #region Forgot Password Brute Force Protection
        Task<string> GetForgotBruteForceProtectionKeyByUserUuidAsync(string recoverySessionUuid);
        Task<bool> IsForgotBruteForceProtectionKeyExistsAsync(string token);
        Task<string> SetForgotBruteForceProtectionKeyAsync(string recoveryUuid, string userTypeId, string userUuid, string? email, string phoneCountryCode, string? phoneNumber);
        Task<string> GetForgotBruteForceProtectionKeyByRecoverySessionUuidAsync(string recoverySessionUuid);
        Task<string> GetForgotBruteForceProtectionSessionUuidByRecoveryTokenAsync(string recoveryToken);

        Task<string?> GetForgotBruteForceProtectionUserUuidByRecoveryTokenAsync(string recoveryToken);
        Task<string?> GetForgotBruteForceProtectionUserTypeIdByRecoveryTokenAsync(string recoveryToken);
        #endregion
    }
}