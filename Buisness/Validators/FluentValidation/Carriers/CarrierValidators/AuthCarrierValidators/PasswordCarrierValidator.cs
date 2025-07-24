using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IPasswordCarrier.Password)))
                .MinimumLength(8)
                    .WithMessage(ValidationMessages.MinLengthFormat(nameof(IPasswordCarrier.Password), 8))
                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IPasswordCarrier.Password), 8))
                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessages.PasswordMustBeAValidFormat(nameof(IPasswordCarrier.Password)));
        }
    }
}
