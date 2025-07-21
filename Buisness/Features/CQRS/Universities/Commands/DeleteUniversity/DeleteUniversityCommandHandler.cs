using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Abstract;
using Core.Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;

namespace Buisness.Features.CQRS.Universities.Commands.DeleteUniversity
{
    public class DeleteUniversityCommandHandler : ICommandHandler<DeleteUniversityCommand, BaseResponse<bool>>
    {
        private readonly IUniversityBuisnessLogicHelper _businessLogicHelper;
        private readonly ILogger<DeleteUniversityCommandHandler> _logger;

        public DeleteUniversityCommandHandler(
            IUniversityBuisnessLogicHelper businessLogicHelper,
            ILogger<DeleteUniversityCommandHandler> logger)
        {
            _businessLogicHelper = businessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteUniversityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Üniversite silme işlemi başlatıldı. ID: {UniversityId}", request.UniversityUuid);

                IBuisnessLogicResult validationResult = BuisnessLogic.Run(
                    await _businessLogicHelper.IsUniversityExistsByUuidAsync(request.UniversityUuid),
                    await _businessLogicHelper.HardDeleteUniversityByUuidAsync(request.UniversityUuid));

                if (validationResult != null)
                {
                    return BaseResponse<bool>.Failure(
                        message: validationResult.Message ?? "Silme Hatası", 
                        statusCode: validationResult.StatusCode);
                }
                return BaseResponse<bool>.Success(
                    data: true,
                    message: "İşlem Başarılı");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite silme sırasında hata oluştu. ID: {UniversityId}", request.UniversityUuid);
                return BaseResponse<bool>.Failure(
                    message: "Üniversite silinirken bir hata oluştu", 
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}