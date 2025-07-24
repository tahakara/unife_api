using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IMiddleNameCarrier : ICarrier
    {
        string? MiddleName { get; set; }
    }
}
