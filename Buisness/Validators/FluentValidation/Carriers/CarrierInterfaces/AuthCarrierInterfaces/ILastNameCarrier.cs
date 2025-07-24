using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface ILastNameCarrier : ICarrier
    {
        string? LastName { get; set; }
    }
}
