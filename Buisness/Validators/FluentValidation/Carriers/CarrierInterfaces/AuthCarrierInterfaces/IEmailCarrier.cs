using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IEmailCarrier : ICarrier
    {
        string? Email { get; set; }
    }
}
