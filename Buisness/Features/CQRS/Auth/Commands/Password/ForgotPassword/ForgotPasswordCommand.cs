using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Features.CQRS.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword
{
    public class ForgotPasswordCommand : ForgotPasswordRequestDto, ICommand<BaseResponse<bool>>
    {
    }
}
