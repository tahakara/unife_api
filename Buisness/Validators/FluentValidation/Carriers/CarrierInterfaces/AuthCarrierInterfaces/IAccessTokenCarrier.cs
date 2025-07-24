using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IAccessTokenCarrier : ICarrier
    {
        string? AccessToken { get; set; }
    }
}
