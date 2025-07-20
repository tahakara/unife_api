using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.Logout;
using Buisness.Features.CQRS.Base;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Universities.Commands.DeleteUniversity;
using System.Windows.Input;

namespace Buisness.Features.CQRS.Auth.Commands.SignIn
{
    public class SignInCommand : SignInRequestDto, ICommand<BaseResponse<SignInResponseDto>>
    {
    }
}
