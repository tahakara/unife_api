using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    public interface INewPasswordCarrier : ICarrier
    {
        string? NewPassword { get; set; }
        string? ConfirmPassword { get; set; }
    }
}
