using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a first name property.
    /// </summary>
    public interface IFirstNameCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        string? FirstName { get; set; }
    }
}
