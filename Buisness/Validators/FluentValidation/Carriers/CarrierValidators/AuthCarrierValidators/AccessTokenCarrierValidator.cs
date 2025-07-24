using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    public class AccessTokenCarrierValidator<T> : AbstractValidator<T>
    where T : IAccessTokenCarrier
    {
        public AccessTokenCarrierValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotNull()
                    .WithMessage(ValidationMessages.RequiredFormat(nameof(IAccessTokenCarrier.AccessToken)))
                .NotEmpty()
                    .WithMessage(ValidationMessages.NotEmptyFormat(nameof(IAccessTokenCarrier.AccessToken)))
                .Must(ValidationHelper.BeAValidJWTBeararToken)
                    .WithMessage(ValidationMessages.InvalidJWTBeararTokenFormat(nameof(IAccessTokenCarrier.AccessToken)));
        }
    }
}
