using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;

namespace Buisness.Features.CQRS.Auth.Commands.SignUp
{
    public class SignUpCommand : TokenCommandBase, ICommand<BaseResponse<bool>>
    {
        public SignUpCommand() { }

        public SignUpCommand(int userType, string? firstName, string? middleNmae, string? lastName, string? email, string? phoneCountryCode, string? phoneNumber, string? password, string? accessToken)
        {
            UserType = userType;
            FirstName = firstName;
            MiddleNmae = middleNmae;
            LastName = lastName;
            Email = email;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            Password = password;
            AccessToken = accessToken;
        }

        public int UserType { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleNmae { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
