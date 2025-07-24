using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
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
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            Include(new AccessTokenCarrierValidator<ChangePasswordCommand>());
            Include(new NewPasswordCarrierValidator<ChangePasswordCommand>());

            RuleFor(x => x.OldPassword)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(ChangePasswordCommand.OldPassword)))
            
                .MinimumLength(8).MaximumLength(100).
                    WithMessage(ValidationMessage.LengthBetweenFormat(nameof(ChangePasswordCommand.OldPassword), 8, 100));
        }
    }
}
