using Buisness.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos
{
    public class ForgotPasswordRequestDto : DtoBase
    {
        // For Command
        public byte? UserTypeId { get; set; } = 0;
        public byte? RecoveryMethodId { get; set; } = 0;
        public string Email { get; set; } = string.Empty;
        public string PhoneCountryCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        // For Internal
        public string? recoveryToken { get; set; } = null;
        public Guid? UserUuid { get; set; } = null;
        public Guid? RecoverySessionUuid { get; set; } = null;

    }
}
