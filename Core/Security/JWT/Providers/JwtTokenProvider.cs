using Core.Security.JWT.Abstractions;
using Core.Security.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Security.JWT.Providers
{
    /// <summary>
    /// Core JWT token provider implementation
    /// </summary>
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly TokenValidationParameters _validationParameters;

        public JwtTokenProvider(IOptions<JwtConfiguration> configuration)
        {
            _configuration = configuration.Value;
            
            if (!_configuration.IsValid())
            {
                throw new ArgumentException("JWT configuration is invalid");
            }

            _tokenHandler = new JwtSecurityTokenHandler();
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey));
            
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateIssuer = true,
                ValidIssuer = _configuration.Issuer,
                ValidateAudience = true,
                ValidAudience = _configuration.Audience,
                ValidateLifetime = true,
                ClockSkew = _configuration.ClockSkew,
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var tokenExpiration = expiration ?? _configuration.AccessTokenExpiration;
            var expires = DateTime.UtcNow.Add(tokenExpiration);

            var allClaims = new List<Claim>(claims)
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.Add(tokenExpiration).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: allClaims,
                expires: expires,
                signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool ValidateToken(string token, out ClaimsPrincipal? principal)
        {
            principal = null;
            
            try
            {
                principal = _tokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);
                return validatedToken is JwtSecurityToken jwtToken && 
                       jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _securityKey,
                ValidateLifetime = false,
                RequireExpirationTime = false
            };

            try
            {
                var principal = _tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                if (validatedToken is not JwtSecurityToken jwtToken || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Claim>? ExtractClaims(string token)
        {
            try
            {
                var jwtToken = _tokenHandler.ReadJwtToken(token);
                return jwtToken.Claims;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateRecoveryToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var tokenExpiration = expiration ?? _configuration.AccessTokenExpiration;
            var expires = DateTime.UtcNow.Add(tokenExpiration);

            var allClaims = new List<Claim>(claims)
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.Add(tokenExpiration).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: allClaims,
                expires: expires,
                signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(token);
        }
    }
}