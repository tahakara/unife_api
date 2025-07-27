using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.AuthCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="IAccessTokenCarrier"/>.
    /// Ensures the access token is not null, not empty, and is a valid JWT Bearer token.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IAccessTokenCarrier"/>.</typeparam>
    public class AccessTokenCarrierValidator<T> : AbstractValidator<T>
        where T : IAccessTokenCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="IAccessTokenCarrier.AccessToken"/> property.
        /// </summary>
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
