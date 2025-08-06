using Buisness.DTOs.AuthDtos.SignInDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Features.CQRS.Universities.Commands.DeleteUniversity;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using System.Windows.Input;

namespace Buisness.Features.CQRS.Auth.Commands.SignIn
{
    public class SignInCommand : ICommand<BaseResponse<SignInResponseDto>>,
        IUserTypeIdCarrier,
        IEmailOrPhoneCarrier,
        IPasswordCarrier
    {
        public byte? UserTypeId { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneCountryCode { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string? Password { get; set; } = null;
    }
}
