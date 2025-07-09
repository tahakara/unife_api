namespace Core.Security.JWT.Configuration
{
    /// <summary>
    /// JWT configuration settings
    /// </summary>
    public class JwtConfiguration
    {
        public const string SectionName = "Jwt";

        /// <summary>
        /// Secret key for signing tokens
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Access token expiration in minutes
        /// </summary>
        public int AccessTokenExpirationMinutes { get; set; } = 15;

        /// <summary>
        /// Refresh token expiration in days
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; } = 7;

        /// <summary>
        /// Clock skew for token validation
        /// </summary>
        public int ClockSkewSeconds { get; set; } = 300; // 5 minutes

        /// <summary>
        /// Algorithm used for signing
        /// </summary>
        public string Algorithm { get; set; } = "HS256";

        /// <summary>
        /// Validates the configuration
        /// </summary>
        /// <returns>True if configuration is valid</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SecretKey) &&
                   !string.IsNullOrEmpty(Issuer) &&
                   !string.IsNullOrEmpty(Audience) &&
                   AccessTokenExpirationMinutes > 0 &&
                   RefreshTokenExpirationDays > 0;
        }

        /// <summary>
        /// Gets access token expiration as TimeSpan
        /// </summary>
        public TimeSpan AccessTokenExpiration => TimeSpan.FromMinutes(AccessTokenExpirationMinutes);

        /// <summary>
        /// Gets refresh token expiration as TimeSpan
        /// </summary>
        public TimeSpan RefreshTokenExpiration => TimeSpan.FromDays(RefreshTokenExpirationDays);

        /// <summary>
        /// Gets clock skew as TimeSpan
        /// </summary>
        public TimeSpan ClockSkew => TimeSpan.FromSeconds(ClockSkewSeconds);
    }
}