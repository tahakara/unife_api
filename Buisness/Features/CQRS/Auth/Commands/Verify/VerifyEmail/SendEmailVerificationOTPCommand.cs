using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;

namespace Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail
{
    public class SendEmailVerificationOTPCommand : ICommand<BaseResponse<bool>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; }
    }
}
