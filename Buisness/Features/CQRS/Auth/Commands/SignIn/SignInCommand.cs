using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Universities.Commands.DeleteUniversity;
using System.Windows.Input;

namespace Buisness.Features.CQRS.Auth.Commands.SignIn
{
    public class SignInCommand : SignCommandBase, ICommand<BaseResponse<bool>>
    {
        public SignInCommand() { }

        public SignInCommand(int userType, string email, string password, string? accessToken)
        {
            UserType = userType;
            Email = email;
            Password = password;
            AccessToken = accessToken;
        }

        public SignInCommand(int userType, string phoneCountryCode, string phoneNumber, string password, string? accessToken)
        {
            UserType = userType;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessToken = accessToken;
        }

        public SignInCommand(int userType, string email, string phoneCountryCode, string phoneNumber, string password, string? accessToken)
        {
            UserType = userType;
            Email = email;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessToken = accessToken;
        }

        public int? UserType { get; set; }
        public string? Email { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
