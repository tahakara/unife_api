using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.PasswordValidators
{
    public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().NotNull()
                .WithMessage("Access token is required.")
                .Must(ValidationHelper.BeAValidJWTBeararToken).WithMessage("Access token must be a valid JWT token.");
            RuleFor(x => x.OldPassword)
                .NotNull().NotEmpty().WithMessage("Old password is required.")
                .MinimumLength(8).MaximumLength(100).WithMessage("Old password must be between 8 and 50 characters long.");
            RuleFor(x => x.NewPassword)
                .NotNull().NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).MaximumLength(100).WithMessage("New password must be between 8 and 50 characters long.")
                .Must(ValidationHelper.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(ValidationHelper.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
                .Must(ValidationHelper.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(ValidationHelper.ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
                .Must(ValidationHelper.BeAValidPassword).WithMessage("Password contains invalid characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotNull().NotEmpty().WithMessage("Password Confirmation is required.")
                .MinimumLength(8).MaximumLength(100).WithMessage("New password must be between 8 and 50 characters long.")
                .Must(ValidationHelper.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(ValidationHelper.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
                .Must(ValidationHelper.ContainDigit).WithMessage("Password must contain at least one digit.")
                .Must(ValidationHelper.ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
                .Must(ValidationHelper.BeAValidPassword).WithMessage("Password contains invalid characters.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("New password and confirmation password must match.");
        }
    }
}
