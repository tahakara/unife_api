using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.Validators.FluentValidation.Validators.Common;
using FluentValidation;
using System.Text.RegularExpressions;

public class SignUpRequestDtoValidator : AbstractValidator<SignUpRequestDto>
{
    public SignUpRequestDtoValidator()
    {
        RuleFor(x => x.UserTypeId)
            .NotNull().NotEmpty().WithMessage("User type ID cannot be null or empty.")
            .GreaterThan(0)
            .WithMessage("User type ID must be greater than 0.");

        RuleFor(x => x.UniversityUuid)
            .Null();

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty.")
            .Length(1, 100).WithMessage("First name must be between 1 and 100 characters.")
            .Must(ValidationHelper.BeAValidFirstName).WithMessage("First name can only contain letters and spaces.");

        RuleFor(x => x.MiddleName)
            .Length(0, 100).WithMessage("Middle name must be between 0 and 100 characters.")
            .Must(ValidationHelper.BeAValidMiddleName).WithMessage("Middle name can only contain letters and spaces.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty.")
            .Length(1, 100).WithMessage("Last name must be between 1 and 100 characters.")
            .Must(ValidationHelper.BeAValidLastName).WithMessage("Last name can only contain letters and spaces.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .MaximumLength(100).WithMessage("Email must be maximum 100 characters.")
            .EmailAddress().WithMessage("Email must be a valid email format.")
            .Must(ValidationHelper.BeAValidEmail).WithMessage("Email is not valid or does not exist.");

        RuleFor(x => x.PhoneCountryCode)
            .NotEmpty().WithMessage("Phone country code cannot be empty.")
            .Must(ValidationHelper.BeAValidCountryCode).WithMessage("Phone country code must start with '+' and contain 1-4 digits.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number cannot be empty.")
            .Must(ValidationHelper.BeAValidPhoneNumber).WithMessage("Phone number must contain only digits (6–15 digits).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100).WithMessage("Password must be maximum 100 characters long.")
            .Must(ValidationHelper.ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
            .Must(ValidationHelper.ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
            .Must(ValidationHelper.ContainDigit).WithMessage("Password must contain at least one digit.")
            .Must(ValidationHelper.ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(ValidationHelper.BeAValidPassword).WithMessage("Password contains invalid characters.");
    }
}