using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a middle name property.
    /// </summary>
    public interface IMiddleNameCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        string? MiddleName { get; set; }
    }
}
