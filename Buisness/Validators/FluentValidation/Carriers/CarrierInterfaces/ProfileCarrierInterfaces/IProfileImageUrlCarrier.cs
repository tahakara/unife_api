using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a profile image URL property.
    /// </summary>
    public interface IProfileImageUrlCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the profile image URL.
        /// </summary>
        string? ProfileImageUrl { get; set; }
    }
}
