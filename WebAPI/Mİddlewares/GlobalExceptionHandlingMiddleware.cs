using Buisness.Features.CQRS.Base.Generic.Response;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace WebAPI.Mİddlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ValidationException validationEx => BaseResponse<object>.Failure(
                    "Validation hatası",
                    validationEx.Errors.Select(e => e.ErrorMessage).ToList(),
                    (int)HttpStatusCode.BadRequest),

                UnauthorizedAccessException => BaseResponse<object>.Failure(
                    "Yetkisiz erişim",
                    statusCode: (int)HttpStatusCode.Unauthorized),

                KeyNotFoundException => BaseResponse<object>.Failure(
                    "Kaynak bulunamadı",
                    statusCode: (int)HttpStatusCode.NotFound),

                _ => BaseResponse<object>.Failure(
                    "Sunucu hatası",
                    new List<string> { "Beklenmeyen bir hata oluştu" },
                    (int)HttpStatusCode.InternalServerError)
            };

            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}