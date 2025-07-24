using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
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
                    .WithMessage(ValidationMessage.RequiredFormat(nameof(IAccessTokenCarrier.AccessToken)))
                .NotEmpty()
                    .WithMessage(ValidationMessage.NotEmptyFormat(nameof(IAccessTokenCarrier.AccessToken)))
                .Must(ValidationHelper.BeAValidJWTBeararToken)
                    .WithMessage(ValidationMessage.InvalidJWTBeararTokenFormat(nameof(IAccessTokenCarrier.AccessToken)));
        }
    }
}
