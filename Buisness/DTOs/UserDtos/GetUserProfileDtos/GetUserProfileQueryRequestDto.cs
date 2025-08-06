using Buisness.DTOs.Base;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.UserDtos.GetUserProfileDtos
{
    public class GetUserProfileQueryRequestDto : RequestDtoBase
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
