using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Concrete.ServiceManager;
using Buisness.DTOs.Common;
using Buisness.DTOs.UniversityDtos;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core.Entities.Base;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Domain.Entities.MainEntities.UniversityModul;

namespace Buisness.Services.EntityRepositoryServices
{
    public class UniversityService : ServiceManagerBase, IUniversityService
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IValidator<CreateUniversityDto> _createValidator;
        private readonly IValidator<UpdateUniversityDto> _updateValidator;

        public UniversityService(
            IUniversityRepository universityRepository,
            IMapper mapper,
            IValidator<CreateUniversityDto> createValidator,
            IValidator<UpdateUniversityDto> updateValidator,
            ILogger<UniversityService> logger,
            IServiceProvider serviceProvider) : base(mapper, logger, serviceProvider)
        {
            _universityRepository = universityRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<SelectUniversityDto> CreateUniversityAsync(CreateUniversityDto dto)
        {
            await LogOperationAsync("CreateUniversity", dto);

            // Validation
            await ValidateAsync(dto);

            // Business Rules
            if (!string.IsNullOrEmpty(dto.UniversityCode))
            {
                var codeExists = await _universityRepository.IsCodeExistsAsync(dto.UniversityCode);
                if (codeExists)
                {
                    throw new InvalidOperationException($"University code '{dto.UniversityCode}' already exists.");
                }
            }

            // Mapping
            var university = _mapper.Map<University>(dto);
            university.UniversityUuid = Guid.NewGuid();
            university.CreatedAt = DateTime.UtcNow;
            university.UpdatedAt = DateTime.UtcNow;

            // Save
            var created = await _universityRepository.AddAsync(university);

            // Return mapped result
            return _mapper.Map<SelectUniversityDto>(created);
        }

        public async Task<SelectUniversityDto> UpdateUniversityAsync(Guid uuid, UpdateUniversityDto dto)
        {
            await LogOperationAsync("UpdateUniversity", new { uuid, dto });

            // Validation
            await ValidateAsync(dto);

            // Get existing
            var existing = await _universityRepository.GetByUuidAsync(uuid);
            if (existing == null)
            {
                throw new InvalidOperationException($"University with UUID '{uuid}' not found.");
            }

            // Business Rules - Check code uniqueness if changed
            if (!string.IsNullOrEmpty(dto.UniversityCode) &&
                dto.UniversityCode != existing.UniversityCode)
            {
                var codeExists = await _universityRepository.IsCodeExistsAsync(dto.UniversityCode);
                if (codeExists)
                {
                    throw new InvalidOperationException($"University code '{dto.UniversityCode}' already exists.");
                }
            }

            // Map updates
            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            // Save
            var updated = await _universityRepository.UpdateAsync(existing);

            return _mapper.Map<SelectUniversityDto>(updated);
        }

        public async Task<bool> DeleteUniversityAsync(Guid uuid)
        {
            await LogOperationAsync("DeleteUniversity", uuid);

            var existing = await _universityRepository.GetByUuidAsync(uuid);
            if (existing == null)
            {
                return false;
            }

            // Soft delete
            existing.IsDeleted = true;
            existing.DeletedAt = DateTime.UtcNow;
            existing.UpdatedAt = DateTime.UtcNow;

            await _universityRepository.UpdateAsync(existing);
            return true;
        }

        public async Task<SelectUniversityDto?> GetUniversityByUuidAsync(Guid uuid)
        {
            var university = await _universityRepository.GetByUuidAsync(uuid);
            return university != null ? _mapper.Map<SelectUniversityDto>(university) : null;
        }

        public async Task<IEnumerable<SelectUniversityDto>> GetUniversitiesByEstablishedYearAsync(int year)
        {
            var universities = await _universityRepository.GetByEstablishedYearAsync(year);
            return _mapper.Map<IEnumerable<SelectUniversityDto>>(universities);
        }

        public async Task<IEnumerable<SelectUniversityDto>> GetUniversitiesByEstablishedYearAsync(int minYear, int maxYear)
        {
            var universities = await _universityRepository.GetByEstablishedYearAsync(minYear, maxYear);
            return _mapper.Map<IEnumerable<SelectUniversityDto>>(universities);
        }

        public async Task<bool> IsUniversityCodeUniqueAsync(string code)
        {
            return !await _universityRepository.IsCodeExistsAsync(code);
        }

        public async Task<PagedResponse<SelectUniversityDto>> GetPagedUniversitiesAsync(int page, int size)
        {
            await LogOperationAsync("GetPagedUniversities", new { page, size });

            // Basic pagination implementation
            var allUniversities = await _universityRepository.UseQueryableAsync(async query =>
                await query.Include(ut => ut.UniversityType).Include(r => r.Region)
                .ToListAsync()
                );
            var allUniversities1 = await _universityRepository.GetAllAsync();
            var totalCount = allUniversities.Count();

            var pagedData = allUniversities
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            var mappedData = _mapper.Map<IEnumerable<SelectUniversityDto>>(pagedData);

            return new PagedResponse<SelectUniversityDto>
            {
                Data = mappedData,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = size
            };
        }

        // ServiceManagerBase'den override edilen metotlar
        public override async Task<bool> ExistsAsync(Guid id)
        {
            var university = await _universityRepository.GetByUuidAsync(id);
            return university != null;
        }

        public override async Task<bool> IsActiveAsync(Guid id)
        {
            var university = await _universityRepository.GetByUuidAsync(id);
            return university?.IsActive == true && university.IsDeleted == false;
        }
    }
}