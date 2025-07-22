using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.UtilityServices.Base.ObjectStorageServices
{
    public interface IOTPCodeService
    {
        Task<bool> SetCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode);
        Task<bool> SetCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode, int attempts, TimeSpan expiration);
        Task<bool> IsCodeExistAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode);
        Task<object?> GetCodeExistBySessionUuidAndUserUuidAsync(string sessionUuid, string userUuid, string otpTypeId);
        Task<bool> RevokeCodeByUserUuid(string userUuid);
        Task<bool> RemoveCodeAsync(string sessionUuid, string userUuid, string otpTypeId, string otpCode);

    }
}
