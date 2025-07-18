using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP
{
    public class VerifyOTPCommand : SignInCommand, ICommand<BaseResponseSuccessWithData<bool>>
    {
        public VerifyOTPCommand() { }

        public VerifyOTPCommand(int userType, string email, string password, string? accessToken, string otpCode)
            : base(userType, email, password, accessToken)
        {
            OTPCode = otpCode;
        }

        public VerifyOTPCommand(int userType, string phoneCountryCode, string phoneNumber, string password, string? accessToken, string otpCode)
            : base(userType, phoneCountryCode, phoneNumber, password, accessToken)
        {
            OTPCode = otpCode;
        }

        public VerifyOTPCommand(int userType, string email, string phoneCountryCode, string phoneNumber, string password, string? accessToken, string otpCode)
            : base(userType, email, phoneCountryCode, phoneNumber, password, accessToken)
        {
            OTPCode = otpCode;
        }
        public string OTPCode { get; set; } = string.Empty;
    }
}
