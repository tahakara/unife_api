using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IEmailCarrier"/>.
    /// Ensures the email is not null, not empty, has a valid format, and meets length requirements.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IEmailCarrier"/>.</typeparam>
    public class EmailCarrierValidator<T> : AbstractValidator<T>
        where T : IEmailCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IEmailCarrier.Email"/> property.
        /// </summary>
        public EmailCarrierValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IEmailCarrier.Email)))

                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IEmailCarrier.Email), 100))

                .EmailAddress()
                    .WithMessage(ValidationMessages.EmailFormat(nameof(IEmailCarrier.Email)))

                .Must(ValidationHelper.BeAValidEmail)
                    .WithMessage(ValidationMessages.NotAccepedEmailFormat(nameof(IEmailCarrier.Email)));
        }
    }
}
