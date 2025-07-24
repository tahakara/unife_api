using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    public class NewPasswordCarrierValidator<T> : AbstractValidator<T>
        where T : INewPasswordCarrier
    {
        public NewPasswordCarrierValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(INewPasswordCarrier.NewPassword)))

                .MinimumLength(8)
                    .WithMessage(ValidationMessage.MinLengthFormat(nameof(INewPasswordCarrier.NewPassword), 8))

                .MaximumLength(100)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(INewPasswordCarrier.NewPassword), 100))

                .Must(ValidationHelper.ContainLowercase)
                    .WithMessage(ValidationMessage.MustContainLowercase(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainUppercase)
                    .WithMessage(ValidationMessage.MustContainUppercase(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainDigit)
                    .WithMessage(ValidationMessage.MustContainDigit(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainAsciiSpecialCharacter)
                    .WithMessage(ValidationMessage.MustContainSpecialChars(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessage.PasswordMustBeAValidFormat(nameof(INewPasswordCarrier.NewPassword)));


            RuleFor(x => x.ConfirmPassword)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(INewPasswordCarrier.ConfirmPassword)))

                .MinimumLength(8)
                    .WithMessage(ValidationMessage.MinLengthFormat(nameof(INewPasswordCarrier.ConfirmPassword), 8))

                .MaximumLength(100)
                    .WithMessage(ValidationMessage.MaxLengthFormat(nameof(INewPasswordCarrier.ConfirmPassword), 100))

                .Must(ValidationHelper.ContainLowercase)
                    .WithMessage(ValidationMessage.MustContainLowercase(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainUppercase)
                    .WithMessage(ValidationMessage.MustContainUppercase(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainDigit)
                    .WithMessage(ValidationMessage.MustContainDigit(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainAsciiSpecialCharacter)
                    .WithMessage(ValidationMessage.MustContainSpecialChars(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessage.PasswordMustBeAValidFormat(nameof(INewPasswordCarrier.ConfirmPassword)));


            // ConfirmPassword must match NewPassword
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                    .WithMessage(ValidationMessage.MissmatchedValuesFormat(nameof(INewPasswordCarrier.NewPassword), nameof(INewPasswordCarrier.ConfirmPassword)));
        }
    }
}
