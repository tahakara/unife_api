using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
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
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(INewPasswordCarrier.NewPassword)))

                .MinimumLength(8)
                    .WithMessage(ValidationMessages.MinLengthFormat(nameof(INewPasswordCarrier.NewPassword), 8))

                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(INewPasswordCarrier.NewPassword), 100))

                .Must(ValidationHelper.ContainLowercase)
                    .WithMessage(ValidationMessages.MustContainLowercase(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainUppercase)
                    .WithMessage(ValidationMessages.MustContainUppercase(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainDigit)
                    .WithMessage(ValidationMessages.MustContainDigit(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.ContainAsciiSpecialCharacter)
                    .WithMessage(ValidationMessages.MustContainSpecialChars(nameof(INewPasswordCarrier.NewPassword)))

                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessages.PasswordMustBeAValidFormat(nameof(INewPasswordCarrier.NewPassword)));


            RuleFor(x => x.ConfirmPassword)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(INewPasswordCarrier.ConfirmPassword)))

                .MinimumLength(8)
                    .WithMessage(ValidationMessages.MinLengthFormat(nameof(INewPasswordCarrier.ConfirmPassword), 8))

                .MaximumLength(100)
                    .WithMessage(ValidationMessages.MaxLengthFormat(nameof(INewPasswordCarrier.ConfirmPassword), 100))

                .Must(ValidationHelper.ContainLowercase)
                    .WithMessage(ValidationMessages.MustContainLowercase(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainUppercase)
                    .WithMessage(ValidationMessages.MustContainUppercase(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainDigit)
                    .WithMessage(ValidationMessages.MustContainDigit(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.ContainAsciiSpecialCharacter)
                    .WithMessage(ValidationMessages.MustContainSpecialChars(nameof(INewPasswordCarrier.ConfirmPassword)))

                .Must(ValidationHelper.BeAValidPassword)
                    .WithMessage(ValidationMessages.PasswordMustBeAValidFormat(nameof(INewPasswordCarrier.ConfirmPassword)));


            // ConfirmPassword must match NewPassword
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                    .WithMessage(ValidationMessages.MissmatchedValuesFormat(nameof(INewPasswordCarrier.NewPassword), nameof(INewPasswordCarrier.ConfirmPassword)));
        }
    }
}
