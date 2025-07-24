using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators;
using Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators;
using Buisness.Validators.FluentValidation.Common;
using FluentValidation;
using System.Text.RegularExpressions;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        Include(new UserTypeIdCarrierValidator<SignUpCommand>());
        Include(new EmailCarrierValidator<SignUpCommand>());
        Include(new PhoneCarrierValidator<SignUpCommand>());
        Include(new PasswordCarrierValidator<SignUpCommand>());
        Include(new FirstNameCarrierValidator<SignUpCommand>());
        Include(new MiddleNameCarrierValidator<SignUpCommand>());
        Include(new LastNameCarrierValidator<SignUpCommand>());
        Include(new UniversityUuidOptionalCarrierValidator<SignUpCommand>());
    }
}