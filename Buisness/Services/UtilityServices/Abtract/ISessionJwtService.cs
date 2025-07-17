using System.Security.Claims;

namespace Buisness.Services.UtilityServices.Abtract
{
    /// <summary>
    /// Session-aware JWT service that integrates with session management
    /// </summary>
    public interface ISessionJwtService
    {
        Task<string> GenerateAccessTokenAsync(string userUuid, string sessionUuid, IEnumerable<Claim>? additionalClaims = null);
        Task<string> GenerateRefreshTokenAsync(string userUuid, string sessionUuid);
        Task<bool> ValidateAndStoreTokenAsync(string userUuid, string sessionUuid, string accessToken, string refreshToken);
        Task<string?> GetRefreshTokenKeyByRefreshTokenPostfixAsync(string refreshTokenPostfix);
        Task<RefreshTokenResult?> RefreshAccessTokenAsync(string refreshToken);
        Task<RefreshTokenResult?> RefreshAccessTokenAsync(string userUuid, string sessionUuid, string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> RevokeTokensAsync(string userUuid, string sessionUuid);
        Task<bool> RevokeUserAllTokensAsync(string userUuid);
        Task<bool> RevokeUserAllTokensExcluededByOne(string userUuid, string excluededSessionUuid);
        string? GetUserUuidFromToken(string token);
        string? GetSessionUuidFromToken(string token);

        Task<string?> GetUserUuidFromTokenAsync(string token);
        Task<string?> GetSessionUuidFromTokenAsync(string token);
    }
}