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
        Task<RefreshTokenResult?> RefreshAccessTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
        Task RevokeTokensAsync(string userUuid, string sessionUuid);
        Task RevokeAllUserTokensAsync(string userUuid);
        string? GetUserUuidFromToken(string token);
        string? GetSessionUuidFromToken(string token);
    }
}