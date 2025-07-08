using Buisness.Abstract.ServicesBase.Base;
using Buisness.DTOs.Common;
using Buisness.DTOs.UniversityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Abstract.ServicesBase
{
    public interface IUniversityService : IServiceManagerBase
    {
        Task<SelectUniversityDto> CreateUniversityAsync(CreateUniversityDto dto);
        Task<SelectUniversityDto> UpdateUniversityAsync(Guid uuid, UpdateUniversityDto dto);
        Task<bool> DeleteUniversityAsync(Guid uuid);
        Task<SelectUniversityDto?> GetUniversityByUuidAsync(Guid uuid);
        Task<IEnumerable<SelectUniversityDto>> GetUniversitiesByEstablishedYearAsync(int year);
        Task<IEnumerable<SelectUniversityDto>> GetUniversitiesByEstablishedYearAsync(int minYear, int maxYear);
        Task<bool> IsUniversityCodeUniqueAsync(string code);
        Task<PagedResponse<SelectUniversityDto>> GetPagedUniversitiesAsync(int page, int size);

    }
}
