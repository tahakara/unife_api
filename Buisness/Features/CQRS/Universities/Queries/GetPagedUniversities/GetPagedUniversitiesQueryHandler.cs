using AutoMapper;
using Buisness.DTOs.Common;
using Buisness.DTOs.UniversityDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Domain.Entities.MainEntities.UniversityModul;
using Buisness.Features.CQRS.Base.Generic.Request.Query;

namespace Buisness.Features.CQRS.Universities.Queries.GetPagedUniversities
{
    public class GetPagedUniversitiesQueryHandler : IQueryHandler<GetPagedUniversitiesQuery, PagedResponse<SelectUniversityDto>>
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagedUniversitiesQueryHandler> _logger;

        public GetPagedUniversitiesQueryHandler(
            IUniversityRepository universityRepository,
            IMapper mapper,
            ILogger<GetPagedUniversitiesQueryHandler> logger)
        {
            _universityRepository = universityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<SelectUniversityDto>> Handle(GetPagedUniversitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                request.ValidateAndSetDefaults();

                _logger.LogInformation("Sayfalı üniversite listesi getirme işlemi başlatıldı. Sayfa: {Page}, Boyut: {Size}, Sıralama: {OrderBy} {OrderDirection}", 
                    request.CurrentPage, request.PageSize, request.OrderBy, request.OrderDirection);

                var skip = (request.CurrentPage - 1) * request.PageSize;

                // Get universities with paging and ordering
                var universities = await _universityRepository.UseQueryableAsync(async query =>
                {
                    var baseQuery = query
                        .Where(u => !u.IsDeleted)
                        .Include(u => u.Region)
                        .Include(u => u.UniversityType);

                    // Apply ordering
                    var orderedQuery = ApplyOrdering(baseQuery, request.OrderBy, request.OrderDirection);

                    return await orderedQuery
                        .Skip(skip)
                        .Take(request.PageSize)
                        .ToListAsync();
                });

                // Get total count
                var totalCount = await _universityRepository.UseQueryableAsync(async query =>
                {
                    return await query
                        .Where(u => !u.IsDeleted)
                        .CountAsync();
                });

                var universityDtos = _mapper.Map<List<SelectUniversityDto>>(universities);

                var pagedResult = new PagedResponse<SelectUniversityDto>
                {
                    Data = universityDtos,
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    TotalCount = totalCount,
                };

                _logger.LogInformation("Sayfalı üniversite listesi başarıyla getirildi. Toplam: {TotalCount}, Sayfa: {Page}", 
                    totalCount, request.CurrentPage);

                return pagedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sayfalı üniversite listesi getirme sırasında hata oluştu");
                throw;
            }
        }

        private static IOrderedQueryable<University> ApplyOrdering(IQueryable<University> query, string orderBy, string orderDirection)
        {
            var isDescending = orderDirection.ToLower() == "desc";

            return orderBy.ToLower() switch
            {
                "universityname" => isDescending 
                    ? query.OrderByDescending(u => u.UniversityName)
                    : query.OrderBy(u => u.UniversityName),
                
                "universitycode" => isDescending
                    ? query.OrderByDescending(u => u.UniversityCode)
                    : query.OrderBy(u => u.UniversityCode),
                
                "establishedyear" => isDescending
                    ? query.OrderByDescending(u => u.EstablishedYear)
                    : query.OrderBy(u => u.EstablishedYear),

                "regionid" => isDescending
                    ? query.OrderByDescending(u => u.RegionId)
                    :query.OrderBy(u => u.RegionId),

                //"regionname" => isDescending
                //    ? query.OrderByDescending(u => u.Region.RegionName)
                //    : query.OrderBy(u => u.Region.RegionName),

                "universitytypenameid" => isDescending
                    ? query.OrderByDescending(u => u.UniversityTypeId)
                    : query.OrderBy(u => u.UniversityTypeId),

                //"universitytypename" => isDescending
                //    ? query.OrderByDescending(u => u.UniversityType.TypeName)
                //    : query.OrderBy(u => u.UniversityType.TypeName),
                
                "createdat" => isDescending
                    ? query.OrderByDescending(u => u.CreatedAt)
                    : query.OrderBy(u => u.CreatedAt),
                
                "updatedat" => isDescending
                    ? query.OrderByDescending(u => u.UpdatedAt)
                    : query.OrderBy(u => u.UpdatedAt),
                
                _ => query.OrderBy(u => u.UniversityUuid) // Default ordering
            };
        }
    }
}