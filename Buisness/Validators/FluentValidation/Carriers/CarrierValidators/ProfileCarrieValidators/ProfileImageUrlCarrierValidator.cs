using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces;
using Core.Utilities.MessageUtility;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.ProfileCarrieValidators
{
    public class ProfileImageUrlCarrierValidator<T> : AbstractValidator<T>
        where T : IProfileImageUrlCarrier
    {
        public ProfileImageUrlCarrierValidator()
        {
            RuleFor(x => x.ProfileImageUrl)
                .NotNull().NotEmpty()
                    .WithMessage(ValidationMessageUtility.NotEmptyFormat(nameof(IProfileImageUrlCarrier.ProfileImageUrl)))

                .Must(ValidationHelper.BeAValidUrl)
                    .WithMessage(ValidationMessageUtility.InvalidUrlFormat(nameof(IProfileImageUrlCarrier.ProfileImageUrl)))

                .MaximumLength(500)
                    .WithMessage(ValidationMessageUtility.MaxLengthFormat(nameof(IProfileImageUrlCarrier.ProfileImageUrl), 500));
        }
    }
}
