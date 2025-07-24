using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class LastNameCarrierValidator<T> : AbstractValidator<T>
    where T : ILastNameCarrier
    {
        public LastNameCarrierValidator()
        {
            RuleFor(x => x.LastName)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(ILastNameCarrier.LastName)))
                .MaximumLength(50)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(ILastNameCarrier.LastName), 50));
        }
    }
}
