using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains phone country code and phone number properties.
    /// </summary>
    public interface IPhoneCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the phone country code.
        /// </summary>
        string? PhoneCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        string? PhoneNumber { get; set; }
    }
}
