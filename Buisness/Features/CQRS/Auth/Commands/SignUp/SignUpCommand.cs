using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;

namespace Buisness.Features.CQRS.Auth.Commands.SignUp
{
    public class SignUpCommand : ICommand<BaseResponse<SignUpResponseDto>>,
        IUserTypeIdCarrier,
        IEmailCarrier,
        IPhoneCarrier,
        IPasswordCarrier,
        IFirstNameCarrier,
        IMiddleNameCarrier,
        ILastNameCarrier,
        IUniversityOptionalCarrier
    {
        public byte? UserTypeId { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneCountryCode { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? FirstName { get; set; } = null;
        public string? MiddleName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? UniversityUuid { get; set; } = null;
    }
}
