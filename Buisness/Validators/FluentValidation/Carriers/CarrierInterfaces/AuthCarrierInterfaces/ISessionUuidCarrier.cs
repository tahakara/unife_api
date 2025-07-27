using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a session UUID property.
    /// </summary>
    public interface ISessionUuidCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the session UUID.
        /// </summary>
        string? SessionUuid { get; set; }
    }
}
