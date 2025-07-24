using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class PasswordCarrierValidator<T> : AbstractValidator<T>
        where T : IPasswordCarrier
    {
        public PasswordCarrierValidator()
        {
            RuleFor(x => x.Password)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IPasswordCarrier.Password)))
                .MinimumLength(8)
                    .WithMessage(ValidationMessage.MinLengthFormat(nameof(IPasswordCarrier.Password), 8))
                .MaximumLength(100)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(IPasswordCarrier.Password), 8))
                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessage.PasswordMustBeAValidFormat(nameof(IPasswordCarrier.Password)));
        }
    }
}
