using Core.Security.JWT.Abstractions;
using Core.Security.JWT.Configuration;
using Core.Security.JWT.Providers;
using Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Security.JWT.Extensions
{
    /// <summary>
    /// JWT service collection extensions
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// Adds JWT services to the service collection
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddJwtCore(this IServiceCollection services, IConfiguration configuration)
        {
            // Validate configuration parameter
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
            }

            // Configure JWT settings
            services.Configure<JwtConfiguration>(options =>
            {
                if (configuration != null)
                {
                    configuration.GetSection(JwtConfiguration.SectionName).Bind(options);
                }
            });
            
            // Add JWT token provider
            services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
            
            return services;
        }

        /// <summary>
        /// Extension method for easier claim creation
        /// </summary>
        /// <param name="userUuid">User UUID</param>
        /// <param name="sessionUuid">Session UUID</param>
        /// <param name="additionalClaims">Additional claims</param>
        /// <returns>Collection of claims</returns>
        public static IEnumerable<Claim> CreateUserSessionClaims(string usertypeId, string userUuid, string sessionUuid, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userUuid),
                new("userUuid", userUuid),
                new("sessionUuid", sessionUuid),
                new("userTypeId", usertypeId),
            };

            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            return claims;
        }

        /// <summary>
        /// Gets user UUID from claims
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <returns>User UUID</returns>
        public static string? GetUserUuid(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("userUuid")?.Value;
        }

        /// <summary>
        /// Gets session UUID from claims
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <returns>Session UUID</returns>
        public static string? GetSessionUuid(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("sessionUuid")?.Value;
        }

        public static string? GetUserTypeId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst("userTypeId")?.Value;
        }
    }
}