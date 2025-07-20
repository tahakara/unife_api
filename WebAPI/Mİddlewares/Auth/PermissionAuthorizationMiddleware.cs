using Buisness.Features.CQRS.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Mİddlewares.Auth
{
    public class PermissionAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PermissionAuthorizationMiddleware> _logger;

        public PermissionAuthorizationMiddleware(RequestDelegate next, ILogger<PermissionAuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userUuid = context.Items["UserUuid"] as string;
            if (string.IsNullOrEmpty(userUuid))
            {
                await WriteFailureResponse(context, "User not authenticated.", StatusCodes.Status401Unauthorized);
                return;
            }

            // TODO: Burada userUuid ile permission kontrolü yap (ör. DB veya Redis üzerinden)
            // Şimdilik örnek olarak izin varmış gibi devam ediyoruz
            bool hasPermission = true;

            if (!hasPermission)
            {
                await WriteFailureResponse(context, "Permission denied.", StatusCodes.Status403Forbidden);
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