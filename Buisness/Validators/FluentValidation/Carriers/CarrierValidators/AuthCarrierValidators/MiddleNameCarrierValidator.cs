using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class MiddleNameCarrierValidator<T> : AbstractValidator<T>
    where T : IMiddleNameCarrier
    {
        public MiddleNameCarrierValidator()
        {
            RuleFor(x => x.MiddleName)
                .MaximumLength(50)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(IMiddleNameCarrier.MiddleName), 50));
        }
    }
}
