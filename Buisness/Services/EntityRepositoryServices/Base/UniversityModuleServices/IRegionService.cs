using Buisness.Services.EntityRepositoryServices.Base;

namespace Buisness.Services.EntityRepositoryServices.Base.UniversityModuleServices
{
    public interface  IRegionService : IServiceManagerBase
    {
        Task<bool> IsRegionIdExistsAsync(int regionId);
        Task<bool> IsRegionNameExistsAsync(string regionName);
        Task<bool> IsRegionCodeAlpha2ExistsAsync(string regionCodeAlpha2);
        Task<bool> IsRegionCodeAlpha3ExistsAsync(string regionCodeAlpha3);
        Task<bool> IsRegionCodeNumericExistsAsync(string regionCodeNumeric);

    }
}
