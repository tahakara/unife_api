using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface ISessionUuidCarrier : ICarrier
    {
        string? SessionUuid { get; set; }
    }
}
