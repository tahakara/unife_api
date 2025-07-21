using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Concrete.ServiceManager;
using Buisness.DTOs.UniversityDtos;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.EntityRepositoryServices
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