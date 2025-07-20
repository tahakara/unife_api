using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;

namespace Buisness.Features.CQRS.Auth.Commands.SignUp
{
    public class SignUpCommand : SignUpRequestDto, ICommand<BaseResponse<SignUpResponseDto>>
    {
    }
}
