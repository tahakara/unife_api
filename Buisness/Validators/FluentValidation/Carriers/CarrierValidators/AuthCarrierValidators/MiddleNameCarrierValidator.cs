using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IMiddleNameCarrier.MiddleName), 50));
        }
    }
}
