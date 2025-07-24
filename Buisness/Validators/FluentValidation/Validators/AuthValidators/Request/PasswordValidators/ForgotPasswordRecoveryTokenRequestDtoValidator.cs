using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.PasswordValidators
{
    public class ForgotPasswordRecoveryTokenCommandValidator :
        AbstractValidator<ForgotPasswordRecoveryTokenCommand>
    {
        public ForgotPasswordRecoveryTokenCommandValidator()
        {
            Include(new NewPasswordCarrierValidator<ForgotPasswordRecoveryTokenCommand>());

            RuleFor(x => x.RecoveryToken)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(ForgotPasswordRecoveryTokenCommand.RecoveryToken)))

                .Must(ValidationHelper.BeAValidJWTToken)
                    .WithMessage(ValidationMessage.InvalidJWTFormat(nameof(ForgotPasswordRecoveryTokenCommand.RecoveryToken)));
        }
    }
}
