using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
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
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            Include(new UserTypeIdCarrierValidator<ForgotPasswordCommand>());
            Include(new EmailOrPhoneCarrierValidator<ForgotPasswordCommand>());

            RuleFor(x => x.RecoveryMethodId)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(ForgotPasswordCommand.RecoveryMethodId)))
                
                .Must(ValidationHelper.BeAValidByte)
                    .WithMessage(ValidationMessage.InvalidByte(nameof(ForgotPasswordCommand.RecoveryMethodId)));
        }
    }
}
