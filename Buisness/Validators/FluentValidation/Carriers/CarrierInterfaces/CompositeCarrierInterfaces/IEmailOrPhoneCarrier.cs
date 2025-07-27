using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains either an email or phone credentials.
    /// </summary>
    public interface IEmailOrPhoneCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        string? Email { get; set; }

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
