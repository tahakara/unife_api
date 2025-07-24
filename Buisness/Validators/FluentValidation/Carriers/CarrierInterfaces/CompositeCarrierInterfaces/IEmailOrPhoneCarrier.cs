using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    public interface IEmailOrPhoneCarrier : ICarrier
    {
        string? Email { get; set; }
        string? PhoneCountryCode { get; set; }
        string? PhoneNumber { get; set; }
    }
}
