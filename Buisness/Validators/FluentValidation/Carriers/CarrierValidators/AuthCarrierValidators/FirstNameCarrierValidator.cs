using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IFirstNameCarrier"/>.
    /// Ensures the first name is not null, not empty, and does not exceed the maximum length.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IFirstNameCarrier"/>.</typeparam>
    public class FirstNameCarrierValidator<T> : AbstractValidator<T>
        where T : IFirstNameCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IFirstNameCarrier.FirstName"/> property.
        /// </summary>
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
