using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a last name property.
    /// </summary>
    public interface ILastNameCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        string? LastName { get; set; }
    }
}
