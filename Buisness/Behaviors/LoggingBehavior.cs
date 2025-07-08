using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Buisness.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("İşlem başladı: {RequestName} - {@Request}", requestName, request);

            try
            {
                var response = await next();
                
                stopwatch.Stop();
                _logger.LogInformation("İşlem tamamlandı: {RequestName} - Süre: {ElapsedMs}ms", 
                    requestName, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "İşlem başarısız: {RequestName} - Süre: {ElapsedMs}ms - Hata: {Error}", 
                    requestName, stopwatch.ElapsedMilliseconds, ex.Message);
                throw;
            }
        }
    }
}