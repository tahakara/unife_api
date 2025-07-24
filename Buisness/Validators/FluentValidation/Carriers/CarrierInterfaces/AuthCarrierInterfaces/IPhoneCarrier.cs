using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IPhoneCarrier : ICarrier
    {
        string? PhoneCountryCode { get; set; }
        string? PhoneNumber { get; set; }
    }
}
