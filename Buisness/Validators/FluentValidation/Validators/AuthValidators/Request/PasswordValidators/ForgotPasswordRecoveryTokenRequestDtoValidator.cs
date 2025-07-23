using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.PasswordValidators
{
    public class ForgotPasswordRecoveryTokenRequestDtoValidator : AbstractValidator<ForgotPasswordRecoveryTokenRequestDto>
    {
        public ForgotPasswordRecoveryTokenRequestDtoValidator()
        {
            RuleFor(x => x.RecoveryToken)
                .NotNull().NotEmpty().WithMessage("Recovery token is required.")
                .Must(ValidationHelper.BeAValidJWTToken).WithMessage("Recovery token must be a valid JWT token.");
        }
    }
}
