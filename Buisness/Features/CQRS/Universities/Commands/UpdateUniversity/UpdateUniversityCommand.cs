using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;

namespace Buisness.Features.CQRS.Universities.Commands.UpdateUniversity
{
    public class UpdateUniversityCommand : UpdateUniversityDto, ICommand<BaseResponse<SelectUniversityDto>>
    {
    }
}
