using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using Buisness.Validators.FluentValidation.Common;
using Buisness.Validators.FluentValidation.ValidationMessages;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    public class NullOrValidAccessTokenCarrierValidator<T> : AbstractValidator<T>
        where T : INullOrValidAccessTokenCarrier
    {
        public NullOrValidAccessTokenCarrierValidator()
        {
            RuleFor(x => x.AccessToken)
                .Must(IsNullOrValidAccessToken)
                    .WithMessage(ValidationMessage.InvalidJWTBeararTokenFormat(nameof(INullOrValidAccessTokenCarrier.AccessToken)));
        }
        private bool IsNullOrValidAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return true;

            var trimmedToken = token.Trim();

            return ValidationHelper.BeAValidJWTBeararToken(token);
        }
    }
}
