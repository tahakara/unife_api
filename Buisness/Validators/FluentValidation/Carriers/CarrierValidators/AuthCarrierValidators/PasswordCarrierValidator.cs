using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IPasswordCarrier"/>.
    /// Ensures the password is not null, not empty, meets length requirements, and is in a valid format.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IPasswordCarrier"/>.</typeparam>
    public class PasswordCarrierValidator<T> : AbstractValidator<T>
        where T : IPasswordCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IPasswordCarrier.Password"/> property.
        /// </summary>
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
