using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains an email property.
    /// </summary>
    public interface IEmailCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        string? Email { get; set; }
    }
}
