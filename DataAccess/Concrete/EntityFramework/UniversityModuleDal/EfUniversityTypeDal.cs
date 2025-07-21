using Core.Database.Base;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Core.Database.Context;
using Domain.Entities.MainEntities.UniversityModul;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Core.Concrete.EntityFramework.UniversityModuleDal
{
    public class EfUniversityTypeDal : EfGenericRepositoryBase<UniversityType, UnifeContext>, IUniversityTypeRepository
    {
        public EfUniversityTypeDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<bool> IsTypeIdExistsAsync(int typeId)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.UniversityTypes
                .AnyAsync(t => t.TypeId == typeId);
        }
    }
}