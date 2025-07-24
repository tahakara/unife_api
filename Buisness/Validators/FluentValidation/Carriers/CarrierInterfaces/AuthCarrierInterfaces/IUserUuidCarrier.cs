using Buisness.Validators.FluentValidation.Carriers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces
{
    public interface IUserUuidCarrier : ICarrier
    {
        string? UserUuid { get; set; }
    }
}
