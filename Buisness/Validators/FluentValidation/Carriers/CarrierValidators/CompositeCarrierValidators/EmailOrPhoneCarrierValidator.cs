using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IEmailOrPhoneCarrier"/>.
    /// Ensures that at least one of email or phone credentials is provided and valid.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IEmailOrPhoneCarrier"/>.</typeparam>
    public class EmailOrPhoneCarrierValidator<T> : AbstractValidator<T>
        where T : IEmailOrPhoneCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailOrPhoneCarrierValidator{T}"/> class.
        /// Sets up validation rules for email and phone credentials.
        /// </summary>
        public EmailOrPhoneCarrierValidator()
        {
            // If email is provided, validate it
            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .MaximumLength(100)
                        .WithMessage(ValidationMessages.MaxLengthFormat(nameof(IEmailOrPhoneCarrier.Email), 100))

                    .EmailAddress()
                        .WithMessage(ValidationMessages.EmailFormat(nameof(IEmailOrPhoneCarrier.Email)))

                    .Must(ValidationHelper.BeAValidEmail)
                        .WithMessage(ValidationMessages.NotAccepedEmailFormat(nameof(IEmailOrPhoneCarrier.Email)));
            });

            // If phone number or country code is provided, validate them
            When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber) || !string.IsNullOrWhiteSpace(x.PhoneCountryCode), () =>
            {
                RuleFor(x => x.PhoneCountryCode)
                    .NotEmpty()
                        .WithMessage(ValidationMessages.NotEmptyFormat(nameof(IEmailOrPhoneCarrier.PhoneCountryCode)))

                    .Must(ValidationHelper.BeAValidCountryCode)
                        .WithMessage(ValidationMessages.PhoneCountryCodeFormat(nameof(IEmailOrPhoneCarrier.PhoneCountryCode)));

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty()
                        .WithMessage(ValidationMessages.NotEmptyFormat(nameof(IEmailOrPhoneCarrier.PhoneNumber)))

                    .Must(ValidationHelper.BeAValidPhoneNumber)
                        .WithMessage(ValidationMessages.PhoneNumberFormat(nameof(IEmailOrPhoneCarrier.PhoneNumber)));
            });

            // Custom validation to ensure at least one of email or phone is provided
            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    bool emailValid = !string.IsNullOrWhiteSpace(dto.Email);
                    bool phoneValid = !string.IsNullOrWhiteSpace(dto.PhoneNumber) && !string.IsNullOrWhiteSpace(dto.PhoneCountryCode);

                    if (!emailValid && !phoneValid)
                    {
                        context.AddFailure(ValidationMessages.EitherEmailOrPhoneCredential(
                            nameof(IEmailOrPhoneCarrier.Email),
                            nameof(IEmailOrPhoneCarrier.PhoneCountryCode),
                            nameof(IEmailOrPhoneCarrier.PhoneNumber)));
                    }
                });
        }
    }
}
