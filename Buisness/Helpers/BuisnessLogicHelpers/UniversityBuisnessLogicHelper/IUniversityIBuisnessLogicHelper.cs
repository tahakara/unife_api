using Buisness.DTOs.UniversityDtos;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper
{
    public interface IUniversityBuisnessLogicHelper : IBuisnessLogicHelper
    {
        Task<IBuisnessLogicResult> IsUniversityNameExistsAsync(
            string universityName,
            Guid? excludeUuid = null,
            bool isDeleted = false);
        Task<IBuisnessLogicResult> IsUniversityCodeAvailableAsync(
            string universityCode,
            Guid? excludeUuid = null,
            bool isDeleted = false);
        Task<IBuisnessLogicResult> IsRegionIdExistsAsync(int regionId);
        Task<IBuisnessLogicResult> IsUniversityTypeIdExistsAsync(int universityTypeId);
        Task<IBuisnessLogicResult> IsEstablishedYearValidAsync(int establishedYear);
        Task<IBuisnessLogicResult> IsWebsiteUrlValidAsync(
            string websiteUrl,
            Guid? excludeUuid = null);
        Task<IBuisnessLogicResult> IsUniversityExistsByUuidAsync(Guid universityUuid);
        Task<IBuisnessLogicDataResult<SelectUniversityDto>> CreateUniversityAsync(
            string universityName,
            string universityCode,
            int regionId,
            int universityTypeId,
            int establishedYear,
            string websiteUrl,
            bool isDeleted = false);
        Task<IBuisnessLogicDataResult<SelectUniversityDto>> UpdateUniversityAsync(
            Guid universityUuid,
            string universityName,
            string universityCode,
            int regionId,
            int universityTypeId,
            int establishedYear,
            string websiteUrl,
            bool isActive = true,
            bool isDeleted = false);
        Task<IBuisnessLogicResult> DeleteUniversityByUuidAsync(Guid universityUuid);
        Task<IBuisnessLogicResult> HardDeleteUniversityByUuidAsync(Guid universityUuid);
    }
}
