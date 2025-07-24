using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(ILastNameCarrier.LastName)))
                .MaximumLength(50)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(ILastNameCarrier.LastName), 50));
        }
    }
}
