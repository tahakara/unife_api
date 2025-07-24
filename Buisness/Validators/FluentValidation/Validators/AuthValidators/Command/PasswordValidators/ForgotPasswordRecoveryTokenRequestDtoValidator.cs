using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Command.PasswordValidators
{
    public class ForgotPasswordRecoveryTokenCommandValidator :
        AbstractValidator<ForgotPasswordRecoveryTokenCommand>
    {
        public ForgotPasswordRecoveryTokenCommandValidator()
        {
            Include(new NewPasswordCarrierValidator<ForgotPasswordRecoveryTokenCommand>());

            RuleFor(x => x.RecoveryToken)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(ForgotPasswordRecoveryTokenCommand.RecoveryToken)))

                .Must(ValidationHelper.BeAValidJWTToken)
                    .WithMessage(ValidationMessages.InvalidJWTFormat(nameof(ForgotPasswordRecoveryTokenCommand.RecoveryToken)));
        }
    }
}
