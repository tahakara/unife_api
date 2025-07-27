using Buisness.Validators.FluentValidation.Carriers.Base;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.CompositeCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that optionally contains a university UUID property.
    /// </summary>
    public interface IUniversityOptionalCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the university UUID. Can be null if not provided.
        /// </summary>
        string? UniversityUuid { get; set; }
    }
}
