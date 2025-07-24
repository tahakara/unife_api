using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    public interface INullOrValidAccessTokenCarrier: ICarrier
    {
        string? AccessToken { get; set; }
    }
}
