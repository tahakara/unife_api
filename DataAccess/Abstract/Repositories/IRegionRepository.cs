using Domain.Entities.MainEntities;
using Domain.Repositories.Abstract.Base;

namespace DataAccess.Abstract.Repositories
{
    public interface IRegionRepository : IGenericRepository<Region>
    {
        Task<bool> IsRegionIdExistsAsync(int regionId);
        Task<bool> IsRegionNameExistsAsync(string regionName);
        Task<bool> IsRegionCodeAlpha2ExistsAsync(string regionCodeAlpha2);
        Task<bool> IsRegionCodeAlpha3ExistsAsync(string regionCodeAlpha3);
        Task<bool> IsRegionCodeNumericExistsAsync(string regionCodeNumeric);

    }
}
