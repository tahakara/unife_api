using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Request.Query;

namespace Buisness.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

        public CachingBehavior(
            IMemoryCache cache, 
            ICacheService cacheService,
            ILogger<CachingBehavior<TRequest, TResponse>> logger)
        {
            _cache = cache;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Query işlemleri için cache kontrolü
            if (request is IQuery<TResponse>)
            {
                return await HandleQuery(request, next);
            }

            // Command işlemleri için cache invalidation
            if (request is ICommand<TResponse>)
            {
                return await HandleCommand(request, next);
            }

            // Diğer request'ler için normal flow
            return await next();
        }

        private async Task<TResponse> HandleQuery(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            //
            // Daha Sonra incelenecek 
            //
            //var cacheKey = GenerateCacheKey(request);
            
            //if (_cache.TryGetValue(cacheKey, out TResponse? cachedResponse) && cachedResponse != null)
            //{
            //    _logger.LogInformation("Cache'den döndürüldü: {CacheKey}", cacheKey);
            //    return cachedResponse;
            //}

            //var response = await next();

            //var cacheOptions = new MemoryCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
            //    SlidingExpiration = TimeSpan.FromMinutes(5),
            //    Priority = CacheItemPriority.Normal
            //};

            //_cache.Set(cacheKey, response, cacheOptions);
            //_logger.LogInformation("Cache'e eklendi: {CacheKey}", cacheKey);

            return response;
        }

        private async Task<TResponse> HandleCommand(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            // Command'ı çalıştır
            var response = await next();

            // Command başarılı ise ilgili cache'leri temizle
            await InvalidateRelatedCaches(request);

            return response;
        }

        private async Task InvalidateRelatedCaches(TRequest request)
        {
            var requestTypeName = typeof(TRequest).Name;
            
            try
            {
                // University ile ilgili cache'leri temizle
                if (requestTypeName.Contains("University"))
                {
                    await _cacheService.InvalidateUniversityCachesAsync();
                    _logger.LogInformation("University cache'leri temizlendi - Command: {CommandType}", requestTypeName);
                }

                // Faculty ile ilgili cache'leri temizle
                if (requestTypeName.Contains("Faculty"))
                {
                    await _cacheService.InvalidateFacultyCachesAsync();
                    _logger.LogInformation("Faculty cache'leri temizlendi - Command: {CommandType}", requestTypeName);
                }

                // Diğer entity'ler için buraya eklenebilir
                // if (requestTypeName.Contains("Academician")) { ... }
                // if (requestTypeName.Contains("Department")) { ... }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache invalidation sırasında hata oluştu - Command: {CommandType}", requestTypeName);
                // Cache invalidation hatası command'ın başarısını etkilemez
            }
        }

        private string GenerateCacheKey(TRequest request)
        {
            var requestName = typeof(TRequest).Name;
            var requestJson = JsonSerializer.Serialize(request);
            return $"{requestName}_{requestJson.GetHashCode()}";
        }
    }
}