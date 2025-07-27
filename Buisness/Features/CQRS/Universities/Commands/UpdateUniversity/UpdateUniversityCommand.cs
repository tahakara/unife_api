using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;

namespace Buisness.Features.CQRS.Universities.Commands.UpdateUniversity
{
    public class UpdateUniversityCommand : UpdateUniversityDto, ICommand<BaseResponse<SelectUniversityDto>>
    {
    }
}
