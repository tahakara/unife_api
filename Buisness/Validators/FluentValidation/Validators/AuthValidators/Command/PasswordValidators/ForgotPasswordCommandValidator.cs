using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Command.PasswordValidators
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            Include(new UserTypeIdCarrierValidator<ForgotPasswordCommand>());
            Include(new EmailOrPhoneCarrierValidator<ForgotPasswordCommand>());

            RuleFor(x => x.RecoveryMethodId)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(ForgotPasswordCommand.RecoveryMethodId)))
                
                .Must(ValidationHelper.BeAValidByte)
                    .WithMessage(ValidationMessages.InvalidByte(nameof(ForgotPasswordCommand.RecoveryMethodId)));
        }
    }
}
