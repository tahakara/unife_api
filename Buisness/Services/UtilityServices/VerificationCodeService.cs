using Core.ObjectStorage.Base;
using Buisness.Services.UtilityServices.Abtract;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class VerificationCodeService : IVerificationCodeService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<VerificationCodeService> _logger;

    public VerificationCodeService(IServiceProvider serviceProvider, ILogger<VerificationCodeService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    private async Task<IObjectStorageConnection> GetVerificationConnectionAsync()
    {
        var verificationFactory = _serviceProvider.GetRequiredKeyedService<IObjectStorageConnectionFactory>("verification");
        return await verificationFactory.CreateConnectionAsync();
    }

    private string GetKey(string type, string userUuid, string key)
        => $"verification:{type}:{userUuid}:{key}";

    public async Task SetCodeAsync(string type, string userUuid, string key, string code, TimeSpan? expiration = null)
    {
        using var connection = await GetVerificationConnectionAsync();
        var redisKey = GetKey(type, userUuid, key);
        await connection.SetStringAsync(redisKey, code, expiration ?? TimeSpan.FromMinutes(10));
    }

    public async Task<string?> GetCodeAsync(string type, string userUuid, string key)
    {
        using var connection = await GetVerificationConnectionAsync();
        var redisKey = GetKey(type, userUuid, key);
        return await connection.GetStringAsync(redisKey);
    }

    public async Task RemoveCodeAsync(string type, string userUuid, string key)
    {
        using var connection = await GetVerificationConnectionAsync();
        var redisKey = GetKey(type, userUuid, key);
        await connection.DeleteAsync(redisKey);
    }
}