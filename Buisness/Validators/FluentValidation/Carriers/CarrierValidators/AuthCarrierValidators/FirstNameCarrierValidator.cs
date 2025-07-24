using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class FirstNameCarrierValidator<T> : AbstractValidator<T>
    where T : IFirstNameCarrier
    {
        public FirstNameCarrierValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IFirstNameCarrier.FirstName)))
                .MaximumLength(50)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IFirstNameCarrier.FirstName), 50));
        }
    }
}
