using Buisness.DTOs.UserDtos.GetUserProfileDtos;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.User.Query
{
    public class GetUserProfileQuery : ICommand<BaseResponse<GetUserProfileResponseDto>>,
        IAccessTokenCarrier
    {
        public string? AccessToken { get; set; } = null;
    }
}
