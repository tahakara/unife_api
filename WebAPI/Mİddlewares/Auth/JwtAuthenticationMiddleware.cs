using Buisness.Features.CQRS.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Mİddlewares.Auth
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;

        public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                await WriteFailureResponse(context, "JWT token is missing.", StatusCodes.Status401Unauthorized);
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // TODO: Burada signature ve expiration doğrulaması eklenmeli
                if (jwtToken == null || jwtToken.ValidTo < DateTime.UtcNow)
                {
                    await WriteFailureResponse(context, "Invalid or expired JWT token.", StatusCodes.Status401Unauthorized);
                    return;
                }

                // UserUuid claimini context'e ekle
                var userUuid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userUuid")?.Value;
                if (string.IsNullOrEmpty(userUuid))
                {
                    await WriteFailureResponse(context, "userUuid claim missing in token.", StatusCodes.Status401Unauthorized);
                    return;
                }

                context.Items["UserUuid"] = userUuid;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "JWT authentication failed.");
                await WriteFailureResponse(context, "JWT authentication failed.", StatusCodes.Status401Unauthorized);
                return;
            }

            await _next(context);
        }

        private static async Task WriteFailureResponse(HttpContext context, string message, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var response = BaseResponse<object>.Failure(message, statusCode: statusCode);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(json);
        }
    }
}