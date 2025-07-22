using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Core.Utilities.BuisnessLogic;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Abstract;
using Domain.Entities;
using Core.Entities.Base;
using MediatR;
using Microsoft.Extensions.Logging;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;

namespace Buisness.Features.CQRS.Universities.Commands.CreateUniversity
{
    public class CreateUniversityCommandHandler : ICommandHandler<CreateUniversityCommand, BaseResponse<SelectUniversityDto>>
    {
        private readonly IUniversityBuisnessLogicHelper _universityBusinessLogicHelper;
        private readonly ILogger<CreateUniversityCommandHandler> _logger;

        public CreateUniversityCommandHandler(
            IUniversityBuisnessLogicHelper universityBusinessLogicHelper,
            ILogger<CreateUniversityCommandHandler> logger)
        {
            _universityBusinessLogicHelper = universityBusinessLogicHelper;
            _logger = logger;
        }

        public async Task<BaseResponse<SelectUniversityDto>> Handle(CreateUniversityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Üniversite oluşturma işlemi başlatıldı. Ad: {UniversityName}", request.UniversityName);

                var validationResult = await BuisnessLogic.Run(
                    () => _universityBusinessLogicHelper.IsUniversityNameExistsAsync(request.UniversityName),
                    // await _universityBusinessLogicHelper.IsUniversityCodeAvailableAsync(request.UniversityCode),
                    () => _universityBusinessLogicHelper.IsRegionIdExistsAsync(request.RegionId),
                    () => _universityBusinessLogicHelper.IsUniversityTypeIdExistsAsync(request.UniversityTypeId),
                    () => _universityBusinessLogicHelper.IsEstablishedYearValidAsync(request.EstablishedYear),
                    () => _universityBusinessLogicHelper.IsWebsiteUrlValidAsync(request.WebsiteUrl)
                );

                if (validationResult != null)
                {
                    return BaseResponse<SelectUniversityDto>.Failure(
                        message: validationResult.Message ?? "Validation hatası",
                        statusCode: validationResult.StatusCode);
                }

                var createResult = await _universityBusinessLogicHelper.CreateUniversityAsync(
                    request.UniversityName,
                    request.UniversityCode,
                    request.RegionId,
                    request.UniversityTypeId,
                    request.EstablishedYear,
                    request.WebsiteUrl);

                if (!createResult.Success)
                {
                    return BaseResponse<SelectUniversityDto>.Failure(
                        createResult.Message ?? "Oluşturma hatası",
                        statusCode: createResult.StatusCode);
                }

                return BaseResponse<SelectUniversityDto>.Success(
                    createResult.Data,
                    createResult.Message ?? "Üniversite başarıyla oluşturuldu",
                    createResult.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite oluşturma sırasında hata oluştu");
                return BaseResponse<SelectUniversityDto>.Failure("Üniversite oluşturulurken bir hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}