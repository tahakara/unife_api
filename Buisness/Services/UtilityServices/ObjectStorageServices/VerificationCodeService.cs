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

    public async Task<bool> RevokeCodeByUserUuid(string userUuid)
    {
        try
        {
            _logger.LogDebug("Revoking all OTP codes for user {UserUuid}", userUuid);
            using var connection = await GetVerificationConnectionAsync();
            var keys = await connection.GetKeysAsync($"OTP:*:{userUuid}:*");

            if (keys.Count == 0)
            {
                _logger.LogInformation("No OTP codes found for user {UserUuid}.", userUuid);
                return true; // No codes to revoke, considered successful
            }

            _logger.LogDebug("Found {Count} OTP codes for user {UserUuid}.", keys.Count, userUuid);

            var tasks = keys.Select(key => connection.DeleteAsync(key)).ToList();
            var results = await Task.WhenAll(tasks);
            bool allRevoked = results.All(result => result);
            if (!allRevoked)
            {
                return false;
            }

            _logger.LogDebug("Successfully revoked all OTP codes for user {UserUuid}.", userUuid);
            return true; // All codes successfully revoked
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while revoking OTP codes for user {UserUuid}.", userUuid);
            return false;
        }
    }
}