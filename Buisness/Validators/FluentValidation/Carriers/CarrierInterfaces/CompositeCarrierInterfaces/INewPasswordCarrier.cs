using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains new password and confirm password properties.
    /// </summary>
    public interface INewPasswordCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        string? NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        string? ConfirmPassword { get; set; }
    }
}
