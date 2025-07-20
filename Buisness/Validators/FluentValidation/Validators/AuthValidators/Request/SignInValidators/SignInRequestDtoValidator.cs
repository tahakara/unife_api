using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.SignInValidators
{
    public class SignInRequestDtoValidator : AbstractValidator<SignInRequestDto>
    {
        public SignInRequestDtoValidator()
        {
            RuleFor(x => x.UserTypeId)
                .NotNull().WithMessage("UserTypeId cannot be null.")
                .Must(ValidationHelper.BeAValidByte).WithMessage("UserTypeId must be between 1 and 255.");

            RuleFor(x => x.UserUuid)
                .Null();

            RuleFor(x => x.SessionUuid)
                .Null();

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


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100).WithMessage("Password must be maximum 100 characters long.");

            RuleFor(x => x.OtpTypeId)
                .Null().WithMessage("OtpTypeId should not be provided.");

            RuleFor(x => x.OtpCode)
                .Null().WithMessage("OtpCode should not be provided.");
        }

    }
}
