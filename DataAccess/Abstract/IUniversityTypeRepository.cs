using Domain.Entities.MainEntities.UniversityModul;
using Domain.Repositories.Abstract.Base;

namespace DataAccess.Abstract
{
    public interface IUniversityTypeRepository : IGenericRepository<UniversityType>
    {
        Task<bool> IsTypeIdExistsAsync(int typeId);
    }
}
