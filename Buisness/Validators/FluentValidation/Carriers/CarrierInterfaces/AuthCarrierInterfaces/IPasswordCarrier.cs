using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IPasswordCarrier : ICarrier
    {
        string? Password { get; set;  }
    }
}
