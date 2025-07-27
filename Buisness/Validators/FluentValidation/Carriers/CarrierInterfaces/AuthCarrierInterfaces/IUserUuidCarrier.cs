using Buisness.Validators.FluentValidation.Carriers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    /// <summary>
    /// Represents a carrier that contains a user UUID property.
    /// </summary>
    public interface IUserUuidCarrier : ICarrier
    {
        /// <summary>
        /// Gets or sets the user UUID.
        /// </summary>
        string? UserUuid { get; set; }
    }
}
