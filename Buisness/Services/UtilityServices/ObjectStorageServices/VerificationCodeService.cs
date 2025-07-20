using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Core.ObjectStorage.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class VerificationCodeService : IOTPCodeService
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

    private string GetKey(string sessionUuid, string userUuid, string otpTypeId, string otpCode)
        => $"OTP:{sessionUuid}:{userUuid}:{otpTypeId}:{otpCode}";

    public async Task<bool> SetCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode)
    {
        try
        {
            _logger.LogDebug("Setting OTP code for session {SessionUuid}, user {UserUuid}, type {OtpTypeId}, code {OtpCode}",
                sessionUuid, userUuid, otpTypeId, otpCode);


            using (var connection = await GetVerificationConnectionAsync())
            {
                var redisKey = GetKey(sessionUuid, userUuid, otpTypeId, otpCode);

                var tokenData = new
                {
                    SessionUuid = sessionUuid,
                    UserUuid = userUuid,
                    OtpTypeId = otpTypeId,
                    OtpCode = otpCode,
                    CreatedAt = DateTime.UtcNow,
                };

                var serializedData = JsonSerializer.Serialize(tokenData);
                bool result = await connection.SetStringAsync(redisKey, serializedData, TimeSpan.FromMinutes(3));

                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while setting the OTP code.");
            return false;
        }

    }

    public async Task<Dictionary<string, string>?> GetCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode)
    {
        using var connection = await GetVerificationConnectionAsync();
        var redisKey = GetKey(sessionUuid, userUuid, otpTypeId, otpCode);
        var value = await connection.GetStringAsync(redisKey);
        if (string.IsNullOrEmpty(value))
            return null;

        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(value);
            return dict;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> IsCodeExistAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode)
    {
        try
        {
            _logger.LogDebug("Checking if OTP code exists for session {SessionUuid}, user {UserUuid}, type {OtpTypeId}, code {OtpCode}",
                sessionUuid, userUuid, otpTypeId, otpCode);
            using var connection = await GetVerificationConnectionAsync();
            var redisKey = GetKey(sessionUuid, userUuid, otpTypeId, otpCode);
            bool exists = await connection.ExistsAsync(redisKey);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while checking the existence of the OTP code.");
            return false;
        }
    }

    public async Task<bool> RemoveCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode)
    {
        try
        {
            _logger.LogDebug("Removing OTP code for session {SessionUuid}, user {UserUuid}, type {OtpTypeId}, code {OtpCode}",
                sessionUuid, userUuid, otpTypeId, otpCode);
            using var connection = await GetVerificationConnectionAsync();
            var redisKey = GetKey(sessionUuid, userUuid, otpTypeId, otpCode);

            bool result = await connection.DeleteAsync(redisKey);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while removing the OTP code.");
            return false;
        }
    }
}