using AutoMapper;
using Buisness.DTOs.UniversityDtos;
using Buisness.Services.EntityRepositoryServices.Base;
using Buisness.Services.EntityRepositoryServices.Base.UniversityModuleServices;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.EntityRepositoryServices.UniversityModuleServices
{
    public class UniversityTypeService : ServiceManagerBase, IUniversityTypeService
    {
        private readonly IUniversityTypeRepository _universityTypeRepository;

        public UniversityTypeService(
            IUniversityTypeRepository universityTypeRepository,
            IMapper mapper,
            IValidator<CreateUniversityDto> createValidator,
            IValidator<UpdateUniversityDto> updateValidator,
            ILogger<UniversityService> logger,
            IServiceProvider serviceProvider) : base(mapper, logger, serviceProvider)
        {
            _universityTypeRepository = universityTypeRepository;
        }

        public async Task<bool> IsTypeIdExistsAsync(int typeId)
        {
            return await _universityTypeRepository.IsTypeIdExistsAsync(typeId);
        }
    }
}