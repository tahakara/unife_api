using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;

namespace Buisness.Features.CQRS.Universities.Commands.DeleteUniversity
{
    public class DeleteUniversityCommand : DeleteUniversityDto, ICommand<BaseResponse<bool>>
    {
    }
}