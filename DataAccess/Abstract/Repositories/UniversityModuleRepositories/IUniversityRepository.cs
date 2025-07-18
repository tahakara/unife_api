using Domain.Repositories.Abstract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Base;
using Domain.Entities.MainEntities.UniversityModul;

namespace DataAccess.Abstract.Repositories.UniversityModuleRepositories
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        Task<University?> GetByUuidAsync(Guid uuid);
        Task<IEnumerable<University>> GetByEstablishedYearAsync(int year);
        Task<IEnumerable<University>> GetByEstablishedYearAsync(int minYear, int maxYear);
        Task<bool> IsCodeExistsAsync(string code);
        Task<bool> IsWebsiteExistsAsync(string websiteUrl);
    }
}
