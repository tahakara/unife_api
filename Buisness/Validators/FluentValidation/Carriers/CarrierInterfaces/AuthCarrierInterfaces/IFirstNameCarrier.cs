using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IFirstNameCarrier : ICarrier
    {
        string? FirstName { get; set; }
    }
}
