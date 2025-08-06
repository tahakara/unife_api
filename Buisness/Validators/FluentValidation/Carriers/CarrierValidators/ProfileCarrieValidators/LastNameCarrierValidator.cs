using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.ProfileCarrieValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="ILastNameCarrier"/>.
    /// Ensures the last name is not null, not empty, and does not exceed the maximum length.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="ILastNameCarrier"/>.</typeparam>
    public class LastNameCarrierValidator<T> : AbstractValidator<T>
        where T : ILastNameCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="ILastNameCarrier.LastName"/> property.
        /// </summary>
        public LastNameCarrierValidator()
        {
            RuleFor(x => x.LastName)
                .NotNull().NotEmpty()
                    .WithMessage(Core.Utilities.MessageUtility.ValidationMessageUtility.RequiredFormat(nameof(ILastNameCarrier.LastName)))

                .MaximumLength(50)
                    .WithMessage(Core.Utilities.MessageUtility.ValidationMessageUtility.MaxLengthFormat(nameof(ILastNameCarrier.LastName), 50));
        }
    }
}
