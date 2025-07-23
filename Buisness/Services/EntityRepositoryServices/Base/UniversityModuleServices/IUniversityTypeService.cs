using Buisness.Services.EntityRepositoryServices.Base;

namespace Buisness.Services.EntityRepositoryServices.Base.UniversityModuleServices
{
    public interface IUniversityTypeService : IServiceManagerBase
    {
        Task<bool> IsTypeIdExistsAsync(int typeId);
    }
}
