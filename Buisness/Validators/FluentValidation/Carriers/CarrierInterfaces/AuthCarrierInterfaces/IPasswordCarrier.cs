using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a password property.
    /// </summary>
    public interface IPasswordCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        string? Password { get; set; }
    }
}
