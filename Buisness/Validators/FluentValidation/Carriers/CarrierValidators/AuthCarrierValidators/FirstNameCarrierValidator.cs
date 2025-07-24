using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.ValidationMessages;
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
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IFirstNameCarrier.FirstName)))
                .MaximumLength(50)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(IFirstNameCarrier.FirstName), 50));
        }
    }
}
