using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : RefreshTokenRequestDto , ICommand<BaseResponse<RefreshTokenResponseDto>>
    {
    }
}
