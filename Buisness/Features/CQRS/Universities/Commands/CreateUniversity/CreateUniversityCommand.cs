using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;

namespace Buisness.Features.CQRS.Universities.Commands.CreateUniversity
{
    public class CreateUniversityCommand : CreateUniversityDto, ICommand<BaseResponse<SelectUniversityDto>>
    {
    }

}