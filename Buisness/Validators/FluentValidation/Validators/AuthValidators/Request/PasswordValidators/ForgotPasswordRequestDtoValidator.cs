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
    public class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestDtoValidator()
        {
            RuleFor(x => x.UserTypeId)
                .NotNull().NotEmpty().WithMessage("UserTypeId is required.")
                .Must(ValidationHelper.BeAValidByte). WithMessage("UserTypeId must be a valid byte value.");
            RuleFor(x => x.RecoveryMethodId)
                .NotNull().NotEmpty().WithMessage("RecoveryMethodId is required.")
                .Must(ValidationHelper.BeAValidByte).WithMessage("RecoveryMethodId must be a valid byte value.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(ValidationHelper.BeAValidEmail).WithMessage("Email must be a valid email address.");

            RuleFor(x => x.PhoneCountryCode)
                .NotEmpty().WithMessage("PhoneCountryCode is required.")
                .Must(ValidationHelper.BeAValidCountryCode).WithMessage("PhoneCountryCode must be a valid phone country code.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber is required.")
                .Must(ValidationHelper.BeAValidPhoneNumber).WithMessage("PhoneNumber must be a valid phone number.")
                .Length(10, 15).WithMessage("PhoneNumber must be between 10 and 15 characters long.");

            // Email kuralları (varsa)
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .MaximumLength(100).WithMessage("Email must be maximum 100 characters.")
                    .EmailAddress().WithMessage("Email must be a valid email format.")
                    .Must(ValidationHelper.BeAValidEmail).WithMessage("Email is not valid or does not exist.");
            });

            // Telefon kuralları (varsa)
            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber) || !string.IsNullOrWhiteSpace(x.PhoneCountryCode), () =>
            {
                RuleFor(x => x.PhoneCountryCode)
                    .NotEmpty().WithMessage("Phone country code cannot be empty.")
                    .Must(ValidationHelper.BeAValidCountryCode).WithMessage("Phone country code must start with '+' and contain 1-4 digits.");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage("Phone number cannot be empty.")
                    .Must(ValidationHelper.BeAValidPhoneNumber).WithMessage("Phone number must contain only digits (6–15 digits).");
            });

            // Email veya Telefon zorunlu
            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    bool emailValid = !string.IsNullOrWhiteSpace(dto.Email);
                    bool phoneValid = !string.IsNullOrWhiteSpace(dto.PhoneNumber) && !string.IsNullOrWhiteSpace(dto.PhoneCountryCode);

                    if (!emailValid && !phoneValid)
                    {
                        context.AddFailure("Either email or phone credentials must be provided.");
                    }
                });
        }
    }
}
