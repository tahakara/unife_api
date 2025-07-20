using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Core.Security.JWT.Abstractions;
using Core.Security.JWT.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class JwtTestController : ControllerBase
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly ISessionJwtService _sessionJwtService;
        private readonly ILogger<JwtTestController> _logger;

        public JwtTestController(
            IJwtTokenProvider jwtTokenProvider,
            ISessionJwtService sessionJwtService,
            ILogger<JwtTestController> logger)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _sessionJwtService = sessionJwtService;
            _logger = logger;
        }

        /// <summary>
        /// Test JWT token generation
        /// </summary>
        /// <param name="request">Token generation request</param>
        /// <returns>Generated tokens</returns>
        [HttpPost("generate-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateTokens([FromBody] GenerateTokenRequest request)
        {
            try
            {
                //var userUuid = request.UserUuid ?? Guid.NewGuid().ToString();
                //var sessionUuid = request.SessionUuid ?? Guid.NewGuid().ToString();

                var userUuid = Guid.NewGuid().ToString();
                var sessionUuid = Guid.NewGuid().ToString();

                // Additional claims
                var additionalClaims = new List<Claim>();
                //if (!string.IsNullOrEmpty(request.Email))
                //{
                //    additionalClaims.Add(new Claim(ClaimTypes.Email, request.Email));
                //}
                //if (!string.IsNullOrEmpty(request.Role))
                //{
                //    additionalClaims.Add(new Claim(ClaimTypes.Role, request.Role));
                //}

                // Generate tokens using session service
                var accessToken = await _sessionJwtService.GenerateAccessTokenAsync(userUuid, sessionUuid, additionalClaims);
                var refreshToken = await _sessionJwtService.GenerateRefreshTokenAsync(userUuid, sessionUuid);

                var response = new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15), // Default expiration
                    TokenType = "Bearer",
                    RedisKeys = new
                    {
                        AccessTokenKey = $"access:{sessionUuid}:{userUuid}",
                        RefreshTokenKey = $"refresh:{refreshToken}"
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token generation failed");
                return BadRequest(new { Error = "Token generation failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Test JWT token validation
        /// </summary>
        /// <param name="request">Token validation request</param>
        /// <returns>Validation result</returns>
        [HttpPost("validate-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
        {
            try
            {
                var isValid = await _sessionJwtService.ValidateTokenAsync(request.Token);

                if (isValid)
                {
                    var userUuid = _sessionJwtService.GetUserUuidFromToken(request.Token);
                    var sessionUuid = _sessionJwtService.GetSessionUuidFromToken(request.Token);

                    // Extract all claims
                    var claims = _jwtTokenProvider.ExtractClaims(request.Token);
                    var claimsDict = claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();

                    var response = new
                    {
                        IsValid = true,
                        UserUuid = userUuid,
                        SessionUuid = sessionUuid,
                        Claims = claimsDict,
                        ValidatedAt = DateTime.UtcNow,
                        RedisKey = $"access:{sessionUuid}:{userUuid}"
                    };

                    return Ok(response);
                }
                else
                {
                    return Ok(new { IsValid = false, Message = "Token is invalid or expired" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed");
                return BadRequest(new { Error = "Token validation failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Test JWT token refresh - Now only requires refresh token
        /// </summary>
        /// <param name="request">Token refresh request</param>
        /// <returns>New access token and refresh token</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenOnlyRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest(new { Error = "Refresh token is required" });
                }

                // parse the

                var refreshResult = await _sessionJwtService.RefreshAccessTokenAsync(request.RefreshToken);

                if (refreshResult != null)
                {
                    var response = new
                    {
                        AccessToken = refreshResult.AccessToken,
                        RefreshToken = refreshResult.RefreshToken,
                        UserUuid = refreshResult.UserUuid,
                        SessionUuid = refreshResult.SessionUuid,
                        RefreshedAt = DateTime.UtcNow,
                        ExpiresAt = refreshResult.ExpiresAt,
                        RefreshExpiresAt = refreshResult.RefreshExpiresAt,
                        TokenType = "Bearer",
                        RedisKeys = new
                        {
                            AccessTokenKey = $"access:{refreshResult.SessionUuid}:{refreshResult.UserUuid}",
                            RefreshTokenKey = $"refresh:{refreshResult.SessionUuid}:{refreshResult.UserUuid}:{refreshResult.RefreshToken}"
                        }
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Error = "Token refresh failed", Message = "Invalid refresh token" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token refresh failed");
                return BadRequest(new { Error = "Token refresh failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Test simple JWT token generation (without session)
        /// </summary>
        /// <param name="request">Simple token request</param>
        /// <returns>Simple JWT token</returns>
        [HttpPost("generate-simple-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateSimpleToken([FromBody] SimpleTokenRequest request)
        {
            try
            {
                var userUuid = request.UserUuid ?? Guid.NewGuid().ToString();
                var sessionUuid = request.SessionUuid ?? Guid.NewGuid().ToString();

                var claims = JwtExtensions.CreateUserSessionClaims(userUuid, sessionUuid);
                var token = _jwtTokenProvider.GenerateAccessToken(claims);

                var response = new
                {
                    Token = token,
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    GeneratedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    Note = "This token is NOT stored in Redis"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Simple token generation failed");
                return BadRequest(new { Error = "Simple token generation failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Test JWT token claims extraction
        /// </summary>
        /// <param name="request">Claims extraction request</param>
        /// <returns>Extracted claims</returns>
        [HttpPost("extract-claims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ExtractClaims([FromBody] ExtractClaimsRequest request)
        {
            try
            {
                var claims = _jwtTokenProvider.ExtractClaims(request.Token);

                if (claims != null)
                {
                    var claimsDict = claims.ToDictionary(c => c.Type, c => c.Value);
                    var response = new
                    {
                        Claims = claimsDict,
                        ClaimsCount = claimsDict.Count,
                        ExtractedAt = DateTime.UtcNow
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { Error = "Claims extraction failed", Message = "Invalid token format" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Claims extraction failed");
                return BadRequest(new { Error = "Claims extraction failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Test JWT token revocation
        /// </summary>
        /// <param name="request">Token revocation request</param>
        /// <returns>Revocation result</returns>
        [HttpPost("revoke-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeTokens([FromBody] RevokeTokensRequest request)
        {
            try
            {
                if (request.RevokeAll)
                {
                    var a = await _sessionJwtService.RevokeUserAllTokensAsync(request.UserUuid);
                    return Ok(new { Message = "All user tokens revoked successfully", RevokedAt = DateTime.UtcNow });
                }
                else
                {
                    var result = await _sessionJwtService.RevokeTokensAsync(request.UserUuid, request.SessionUuid);
                    if (result)
                    {
                        return Ok(new { Message = "Session tokens revoked successfully", RevokedAt = DateTime.UtcNow });
                    }

                    return BadRequest(new { Error = "Token revocation failed", Message = "Invalid user or session UUID" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token revocation failed");
                return BadRequest(new { Error = "Token revocation failed", Details = ex.Message });
            }
        }

        /// <summary>
        /// Get JWT configuration info
        /// </summary>
        /// <returns>JWT configuration details</returns>
        [HttpGet("config-info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetConfigInfo()
        {
            var configInfo = new
            {
                Message = "JWT configuration is active",
                TokenType = "Bearer",
                Algorithm = "HS256",
                DefaultExpiration = "15 minutes",
                RefreshTokenExpiration = "7 days",
                ClockSkew = "5 minutes",
                RedisKeyStructure = new
                {
                    AccessToken = "access:{sessionId}:{userId}",
                    RefreshToken = "refresh:{refreshToken}"
                },
                CheckedAt = DateTime.UtcNow
            };

            return Ok(configInfo);
        }
    }

    // Request/Response Models
    public class GenerateTokenRequest
    {
        public string? UserUuid { get; set; }
        public string? SessionUuid { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }

    public class ValidateTokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string UserUuid { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenOnlyRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class SimpleTokenRequest
    {
        public string? UserUuid { get; set; }
        public string? SessionUuid { get; set; }
    }

    public class ExtractClaimsRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class RevokeTokensRequest
    {
        public string UserUuid { get; set; } = string.Empty;
        public string SessionUuid { get; set; } = string.Empty;
        public bool RevokeAll { get; set; } = false;
    }
}