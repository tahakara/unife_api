using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains an access token property.
    /// </summary>
    public interface IAccessTokenCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        string? AccessToken { get; set; }
    }
}
