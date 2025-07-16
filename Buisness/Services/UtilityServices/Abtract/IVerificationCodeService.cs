using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.UtilityServices.Abtract
{
    public interface IVerificationCodeService
    {
        Task SetCodeAsync(string type, string userUuid, string key, string code, TimeSpan? expiration = null);
        Task<string?> GetCodeAsync(string type, string userUuid, string key);
        Task RemoveCodeAsync(string type, string userUuid, string key);
    }
}
