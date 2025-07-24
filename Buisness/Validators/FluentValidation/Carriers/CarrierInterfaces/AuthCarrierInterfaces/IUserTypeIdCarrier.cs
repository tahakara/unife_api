using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IUserTypeIdCarrier : ICarrier
    {
        byte? UserTypeId { get; set; }
    }
}
