using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    public class EmailOrPhoneCarrierValidator<T> : AbstractValidator<T>
        where T : IEmailOrPhoneCarrier
    {
        public EmailOrPhoneCarrierValidator()
        {
            // Email kuralları (varsa)
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

            // Telefon kuralları (varsa)
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

            // Email veya Telefon zorunlu
            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    bool emailValid = !string.IsNullOrWhiteSpace(dto.Email);
                    bool phoneValid = !string.IsNullOrWhiteSpace(dto.PhoneNumber) && !string.IsNullOrWhiteSpace(dto.PhoneCountryCode);

                    if (!emailValid && !phoneValid)
                    {
                        context.AddFailure(ValidationMessages.EitherEmailOrPhoneCredential(nameof(IEmailOrPhoneCarrier.Email),                          nameof(IEmailOrPhoneCarrier.PhoneCountryCode), nameof(IEmailOrPhoneCarrier.PhoneNumber)));
                    }
                });
        }
    }
}
