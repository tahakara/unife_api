using Buisness.Features.CQRS.Base;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using Domain.Enums.EntityEnums.MainEntityEnums.AuthorizationEnums.SecurityEventEnums;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordRecoveryTokenCommand : 
        INewPasswordCarrier, 
        ICommand<BaseResponse<bool>>
    {
        public string? RecoveryToken { get; set; } = null;
        public string? NewPassword { get; set; } = null;
        public string? ConfirmPassword { get; set; } = null;
    }
}
