using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains an access token property which can be null or a valid JWT Bearer token.
    /// </summary>
    public interface INullOrValidAccessTokenCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the access token. Can be null or a valid JWT Bearer token.
        /// </summary>
        string? AccessToken { get; set; }
    }
}
