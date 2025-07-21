using AutoMapper;
using Buisness.DTOs.UniversityDtos;
using Buisness.Features.CQRS.Base;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Buisness.Features.CQRS.Universities.Queries.GetUniversityById
{
    public class GetUniversityByIdQueryHandler : IQueryHandler<GetUniversityByIdQuery, BaseResponse<SelectUniversityDto>>
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUniversityByIdQueryHandler> _logger;

        public GetUniversityByIdQueryHandler(
            IUniversityRepository universityRepository,
            IMapper mapper,
            ILogger<GetUniversityByIdQueryHandler> logger)
        {
            _universityRepository = universityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<SelectUniversityDto>> Handle(GetUniversityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Üniversite getirme işlemi başlatıldı. ID: {UniversityId}", request.UniversityId);

                // Updated to use the correct method signature for GetAsync
                var university = await _universityRepository.GetAsync(
                    u => u.UniversityUuid == request.UniversityId && !u.IsDeleted);

                if (university == null)
                {
                    _logger.LogWarning("Üniversite bulunamadı. ID: {UniversityId}", request.UniversityId);
                    return BaseResponse<SelectUniversityDto>.Failure("Üniversite bulunamadı", statusCode: 404);
                }

                // Explicitly load related properties if needed
                if (university.Region == null || university.UniversityType == null)
                {
                    await _universityRepository.UseQueryableAsync(async query =>
                    {
                        var entity = await query
                            .Where(u => u.UniversityUuid == request.UniversityId)
                            .Include(u => u.Region)
                            .Include(u => u.UniversityType)
                            .FirstOrDefaultAsync();
                        return entity;
                    });
                }

                var universityDto = _mapper.Map<SelectUniversityDto>(university);

                _logger.LogInformation("Üniversite başarıyla getirildi. ID: {UniversityId}", request.UniversityId);

                return BaseResponse<SelectUniversityDto>.Success(universityDto, "Üniversite başarıyla getirildi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite getirme sırasında hata oluştu. ID: {UniversityId}", request.UniversityId);
                return BaseResponse<SelectUniversityDto>.Failure("Üniversite getirilirken bir hata oluştu",
                    new List<string> { ex.Message }, 500);
            }
        }
    }
}