using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Abstract;
using Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;

namespace Buisness.Features.CQRS.Universities.Commands.UpdateUniversity
{
    public class UpdateUniversityCommandHandler : ICommandHandler<UpdateUniversityCommand, BaseResponse<SelectUniversityDto>>
    {
        private readonly IUniversityBuisnessLogicHelper _universityBusinessLogicHelper;
        private readonly ILogger<UpdateUniversityCommandHandler> _logger;

        public UpdateUniversityCommandHandler(
            IUniversityBuisnessLogicHelper universityBusinessLogicHelper,
            ILogger<UpdateUniversityCommandHandler> logger)
        {
            _universityBusinessLogicHelper = universityBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<SelectUniversityDto>> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Üniversite güncelleme işlemi başlatıldı. ID: {UniversityId}", request.UniversityUuid);

                var validationResult = await BuisnessLogic.Run(
                       () => _universityBusinessLogicHelper.IsUniversityExistsByUuidAsync(request.UniversityUuid),
                       () => _universityBusinessLogicHelper.IsUniversityNameExistsAsync(request.UniversityName, request.UniversityUuid),
                       /// await _universityBusinessLogicHelper.IsUniversityCodeAvailableAsync(request.UniversityCode, request.UniversityUuid),
                       () => _universityBusinessLogicHelper.IsRegionIdExistsAsync(request.RegionId),
                       () => _universityBusinessLogicHelper.IsUniversityTypeIdExistsAsync(request.UniversityTypeId),
                       () => _universityBusinessLogicHelper.IsEstablishedYearValidAsync(request.EstablishedYear),
                       () => _universityBusinessLogicHelper.IsWebsiteUrlValidAsync(request.WebsiteUrl, request.UniversityUuid));

                if (validationResult != null)
                {
                    _logger.LogWarning("Üniversite güncelleme sırasında validation hatası: {Message}", validationResult.Message);
                    return BaseResponse<SelectUniversityDto>.Failure(
                        message: validationResult.Message ?? "Validation hatası",
                        statusCode: validationResult.StatusCode);
                }

                IBuisnessLogicDataResult<SelectUniversityDto> updateResult = await _universityBusinessLogicHelper.UpdateUniversityAsync(
                    request.UniversityUuid, 
                    request.UniversityName, 
                    request.UniversityCode, 
                    request.RegionId, 
                    request.UniversityTypeId, 
                    request.EstablishedYear, 
                    request.WebsiteUrl, 
                    request.IsActive);

                if (!updateResult.Success)
                {
                    _logger.LogWarning("Üniversite güncelleme sırasında hata: {Message}", updateResult.Message);
                    return BaseResponse<SelectUniversityDto>.Failure(
                        updateResult.Message ?? "Üniversite güncellenirken bir hata oluştu",
                        statusCode: updateResult.StatusCode);
                }

                _logger.LogInformation("Üniversite başarıyla güncellendi. ID: {UniversityId}", request.UniversityUuid);

                return BaseResponse<SelectUniversityDto>.Success(
                        data: updateResult.Data,
                        message: updateResult.Message ?? "Üniversite başarıyla güncellendi",
                        statusCode: updateResult.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite güncelleme sırasında hata oluştu. ID: {UniversityId}", request.UniversityUuid);
                return BaseResponse<SelectUniversityDto>.Failure(
                    message: "Üniversite güncellenirken bir hata oluştu",
                    errors: new List<string> { ex.Message }, 
                    statusCode: 500);
            }
        }
    }
}
