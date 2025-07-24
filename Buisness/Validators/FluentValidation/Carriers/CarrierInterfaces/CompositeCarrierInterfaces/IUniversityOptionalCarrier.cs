using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    public interface IUniversityOptionalCarrier : ICarrier
    {
        string? UniversityUuid { get; set; }
    }
}
