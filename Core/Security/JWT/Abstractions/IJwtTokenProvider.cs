using System.Security.Claims;

namespace Core.Security.JWT.Abstractions
{
    /// <summary>
    /// Core JWT token provider interface
    /// </summary>
    public interface IJwtTokenProvider
    {
        /// <summary>
        /// Generates an access token with specified claims
        /// </summary>
        /// <param name="claims">Claims to include in the token</param>
        /// <param name="expiration">Token expiration time</param>
        /// <returns>Generated JWT token</returns>
        string GenerateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null);

        /// <summary>
        /// Generates a refresh token
        /// </summary>
        /// <returns>Generated refresh token</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Validates a JWT token
        /// </summary>
        /// <param name="token">Token to validate</param>
        /// <param name="principal">Output principal if valid</param>
        /// <returns>True if token is valid</returns>
        bool ValidateToken(string token, out ClaimsPrincipal? principal);

        /// <summary>
        /// Gets principal from expired token (for refresh scenarios)
        /// </summary>
        /// <param name="token">Expired token</param>
        /// <returns>Claims principal if token structure is valid</returns>
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        /// <summary>
        /// Extracts claims from a token without validation
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>Claims if token is readable</returns>
        IEnumerable<Claim>? ExtractClaims(string token);
    }
}