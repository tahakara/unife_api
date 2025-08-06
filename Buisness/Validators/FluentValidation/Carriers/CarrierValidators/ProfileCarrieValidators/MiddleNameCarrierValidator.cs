using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces;
using Core.Utilities.MessageUtility;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.ProfileCarrieValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IMiddleNameCarrier"/>.
    /// Ensures the middle name does not exceed the maximum allowed length.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IMiddleNameCarrier"/>.</typeparam>
    public class MiddleNameCarrierValidator<T> : AbstractValidator<T>
        where T : IMiddleNameCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiddleNameCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IMiddleNameCarrier.MiddleName"/> property.
        /// </summary>
        public MiddleNameCarrierValidator()
        {
            RuleFor(x => x.MiddleName)
                .MaximumLength(50)
                    .WithMessage(ValidationMessageUtility.MaxLengthFormat(nameof(IMiddleNameCarrier.MiddleName), 50));
        }
    }
}
