using Buisness.Validators.Common;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces;
using FluentValidation;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierValidators.CompositeCarrierValidators
{
    /// <summary>
    /// Validator for types implementing <see cref="INullOrValidAccessTokenCarrier"/>.
    /// Ensures the access token is either null/empty or a valid JWT Bearer token.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="INullOrValidAccessTokenCarrier"/>.</typeparam>
    public class NullOrValidAccessTokenCarrierValidator<T> : AbstractValidator<T>
        where T : INullOrValidAccessTokenCarrier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullOrValidAccessTokenCarrierValidator{T}"/> class.
        /// Sets up validation rules for the <see cref="INullOrValidAccessTokenCarrier.AccessToken"/> property.
        /// </summary>
        public NullOrValidAccessTokenCarrierValidator()
        {
            RuleFor(x => x.AccessToken)
                .Must(IsNullOrValidAccessToken)
                    .WithMessage(ValidationMessages.InvalidJWTBeararTokenFormat(nameof(INullOrValidAccessTokenCarrier.AccessToken)));
        }

        /// <summary>
        /// Checks if the access token is either null/empty or a valid JWT Bearer token.
        /// </summary>
        /// <param name="token">The access token to validate.</param>
        /// <returns>True if the token is null/empty or valid; otherwise, false.</returns>
        private bool IsNullOrValidAccessToken(string? token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrEmpty(token))
                return true;

            var trimmedToken = token.Trim();

            return ValidationHelper.BeAValidJWTBeararToken(token);
        }
    }
}
