using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a user type ID property.
    /// </summary>
    public interface IUserTypeIdCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the user type ID.
        /// </summary>
        byte? UserTypeId { get; set; }
    }
}
