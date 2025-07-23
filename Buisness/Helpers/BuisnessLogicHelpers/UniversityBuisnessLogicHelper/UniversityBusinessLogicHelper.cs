using AutoMapper;
using Buisness.DTOs.UniversityDtos;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using Core.Entities.Base;
using Domain.Entities;
//using Domain.Entities.MainEntities.UniversityModul;
using Microsoft.Extensions.Logging;
using Core.Utilities.BuisnessLogic.BuisnessLogicResults.DataResults;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Domain.Entities.MainEntities.UniversityModul;
using Buisness.Services.EntityRepositoryServices.Base.UniversityModuleServices;

namespace Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper
{
    public class UniversityBusinessLogicHelper : IUniversityBuisnessLogicHelper
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly IUniversityTypeService _universityTypeService;
        private readonly IRegionService _regionService;
        private readonly IMapper _mapper;
        private readonly ILogger<UniversityBusinessLogicHelper> _logger;

        public UniversityBusinessLogicHelper(
            IUniversityRepository universityRepository,
            IUniversityTypeService universityTypeService,
            IRegionService regionService,
            IMapper mapper,
            ILogger<UniversityBusinessLogicHelper> logger)
        {
            _universityRepository = universityRepository;
            _universityTypeService = universityTypeService;
            _regionService = regionService;
            _mapper = mapper;
            _logger = logger;
        }

        #region Name Validation

        /// <summary>
        /// Üniversite adının mevcut olup olmadığını kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsUniversityNameExistsAsync(
            string universityName, 
            Guid? excludeUuid = null, 
            bool isDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(universityName))
            {
                return new BuisnessLogicErrorResult("Üniversite adı boş olamaz", 400);
            }

            var normalizedName = universityName.Trim().ToUpper();

            var university = excludeUuid.HasValue
                ? await _universityRepository.GetAsync(u => 
                    u.UniversityName == normalizedName && 
                    u.UniversityUuid != excludeUuid.Value &&
                    u.IsDeleted == isDeleted)
                : await _universityRepository.GetAsync(u => 
                    u.UniversityName == normalizedName && 
                    u.IsDeleted == isDeleted);

            if (university != null)
            {
                return new BuisnessLogicErrorResult("Bu isimde bir üniversite zaten mevcut", 409);
            }

            return new BuisnessLogicSuccessResult("Üniversite adı kontrolü başarılı");
        }

        #endregion

        #region Code Validation

        /// <summary>
        /// Üniversite kodunun uygunluğunu kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsUniversityCodeAvailableAsync(
            string universityCode, 
            Guid? excludeUuid = null, 
            bool isDeleted = false)
        {
            if (string.IsNullOrWhiteSpace(universityCode))
            {
                return new BuisnessLogicErrorResult("Üniversite kodu boş olamaz", 400);
            }

            var normalizedCode = universityCode.Trim().ToUpper();

            // Code format validation
            if (normalizedCode.Length < 2 || normalizedCode.Length > 10)
            {
                return new BuisnessLogicErrorResult("Üniversite kodu 2-10 karakter arasında olmalıdır", 400);
            }

            var university = excludeUuid.HasValue
                ? await _universityRepository.GetAsync(u => 
                    u.UniversityCode == normalizedCode && 
                    u.UniversityUuid != excludeUuid.Value &&
                    u.IsDeleted == isDeleted)
                : await _universityRepository.GetAsync(u => 
                    u.UniversityCode == normalizedCode && 
                    u.IsDeleted == isDeleted);

            if (university != null)
            {
                return new BuisnessLogicErrorResult("Bu kodda bir üniversite zaten mevcut", 409);
            }

            return new BuisnessLogicSuccessResult("Üniversite kodu kontrolü başarılı");
        }

        #endregion

        #region Region Validation

        /// <summary>
        /// Bölge ID'sinin varlığını kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsRegionIdExistsAsync(int regionId)
        {
            if (regionId <= 0)
            {
                return new BuisnessLogicErrorResult("Geçersiz bölge ID", 400);
            }

            var exists = await _regionService.IsRegionIdExistsAsync(regionId);
            if (!exists)
            {
                return new BuisnessLogicErrorResult("Belirtilen bölge ID'si bulunamadı", 404);
            }

            return new BuisnessLogicSuccessResult("Bölge ID kontrolü başarılı");
        }

        #endregion

        #region University Type Validation

        /// <summary>
        /// Üniversite tipi ID'sinin varlığını kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsUniversityTypeIdExistsAsync(int universityTypeId)
        {
            if (universityTypeId <= 0)
            {
                return new BuisnessLogicErrorResult("Geçersiz üniversite tipi ID", 400);
            }

            var exists = await _universityTypeService.IsTypeIdExistsAsync(universityTypeId);
            if (!exists)
            {
                return new BuisnessLogicErrorResult("Belirtilen üniversite tipi ID'si bulunamadı", 404);
            }

            return new BuisnessLogicSuccessResult("Üniversite tipi ID kontrolü başarılı");
        }

        #endregion

        #region Established Year Validation

        /// <summary>
        /// Kuruluş yılının geçerliliğini kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsEstablishedYearValidAsync(int establishedYear)
        {
            var currentYear = DateTime.UtcNow.Year;
            const int minimumYear = 1000; // İlk üniversiteler 11. yüzyılda kuruldu

            if (establishedYear < minimumYear || establishedYear > currentYear)
            {
                return new BuisnessLogicErrorResult(
                    $"Kuruluş yılı {minimumYear} ile {currentYear} arasında olmalıdır", 400);
            }

            return await Task.FromResult(new BuisnessLogicSuccessResult("Kuruluş yılı kontrolü başarılı"));
        }

        #endregion

        #region Website URL Validation

        /// <summary>
        /// Web sitesi URL'sinin geçerliliğini kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsWebsiteUrlValidAsync(
            string websiteUrl, 
            Guid? excludeUuid = null)
        {
            if (string.IsNullOrWhiteSpace(websiteUrl))
            {
                return new BuisnessLogicSuccessResult("Web sitesi URL'si boş, kontrol atlandı");
            }

            var normalizedUrl = websiteUrl.Trim().ToLower();

            // URL format validation
            if (!Uri.TryCreate(normalizedUrl, UriKind.Absolute, out Uri? uri))
            {
                return new BuisnessLogicErrorResult("Geçersiz web sitesi URL formatı", 400);
            }

            // Protocol validation
            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                return new BuisnessLogicErrorResult("Web sitesi URL'si http veya https protokolü kullanmalıdır", 400);
            }

            // GEÇICI: Educational domain validation'ı kapat
             string domain = uri.Host.ToLower();
            if (!IsEducationalDomain(domain))
            {
                return new BuisnessLogicErrorResult(
                    "Üniversite web sitesi eğitim kurumu domain'i (.edu, .edu.tr, .ac.uk vb.) kullanmalıdır", 400);
            }

            // Check for existing URL
            try
            {
                var existingUniversity = excludeUuid.HasValue
                    ? await _universityRepository.GetAsync(u => 
                        u.WebsiteUrl == normalizedUrl && 
                        u.UniversityUuid != excludeUuid.Value &&
                        u.IsDeleted == false)
                    : await _universityRepository.GetAsync(u => 
                        u.WebsiteUrl == normalizedUrl && 
                        u.IsDeleted == false);

                if (existingUniversity != null)
                {
                    return new BuisnessLogicErrorResult("Bu web sitesi URL'sine sahip üniversite zaten mevcut", 409);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Web sitesi URL kontrolü sırasında hata oluştu. URL: {WebsiteUrl}", normalizedUrl);
                // Hata durumunda devam et
            }

            return new BuisnessLogicSuccessResult("Web sitesi URL kontrolü başarılı");
        }

        /// <summary>
        /// Eğitim kurumu domain'i olup olmadığını kontrol eder
        /// </summary>
        private static bool IsEducationalDomain(string domain)
        {
            var educationalTlds = new[]
            {
                ".edu", ".edu.tr", ".edu.au", ".edu.br", ".edu.cn", 
                ".edu.de", ".edu.mx", ".edu.sg", ".edu.my",
                ".ac.uk", ".ac.jp", ".ac.kr", ".ac.nz", ".ac.za",
                ".university", ".college"
            };

            return educationalTlds.Any(tld => domain.EndsWith(tld, StringComparison.OrdinalIgnoreCase)) ||
                   domain.Contains("university") || domain.Contains("college") || 
                   domain.Contains("edu") || domain.Contains("univ");
        }

        #endregion

        #region University Existence Validation

        /// <summary>
        /// Üniversitenin varlığını UUID ile kontrol eder
        /// </summary>
        public async Task<IBuisnessLogicResult> IsUniversityExistsByUuidAsync(Guid universityUuid)
        {
            var university = await _universityRepository.GetAsync(u => 
                u.UniversityUuid == universityUuid && 
                u.IsDeleted == false);

            if (university == null)
            {
                return new BuisnessLogicErrorResult("Üniversite bulunamadı", 404);
            }

            return new BuisnessLogicSuccessResult("Üniversite varlık kontrolü başarılı");
        }

        #endregion

        #region Database Operations

        /// <summary>
        /// Yeni üniversiteyi veritabanına ekler
        /// </summary>
        public async Task<IBuisnessLogicDataResult<SelectUniversityDto>> CreateUniversityAsync(
            string universityName,
            string universityCode,
            int regionId,
            int universityTypeId,
            int establishedYear,
            string websiteUrl,
            bool isDeleted = false)
        {
            try
            {
                var university = new University
                {
                    UniversityUuid = Guid.NewGuid(),
                    UniversityName = universityName.Trim().ToUpper(),
                    UniversityCode = universityCode.Trim().ToUpper(),
                    RegionId = regionId,
                    UniversityTypeId = universityTypeId,
                    EstablishedYear = establishedYear,
                    WebsiteUrl = websiteUrl?.Trim().ToLower(),
                    IsActive = true,
                    IsDeleted = false
                };

                var createdUniversity = await _universityRepository.AddAsync(university);

                var universityDto = _mapper.Map<SelectUniversityDto>(createdUniversity);

                _logger.LogInformation("Üniversite başarıyla oluşturuldu. ID: {UniversityId}", 
                    createdUniversity.UniversityUuid);

                return new BuisnessLogicSuccessDataResult<SelectUniversityDto>(
                    universityDto, 201, "Üniversite başarıyla oluşturuldu");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite oluşturma sırasında veritabanı hatası");
                return new BuisnessLogicErrorDataResult<SelectUniversityDto>(
                    "Üniversite oluşturulurken veritabanı hatası oluştu", 500);
            }
        }
        public async Task<IBuisnessLogicDataResult<SelectUniversityDto>> UpdateUniversityAsync(
            Guid universityUuid,
            string universityName,
            string universityCode,
            int regionId,
            int universityTypeId,
            int establishedYear,
            string websiteUrl,
            bool isActive = true,
            bool isDeleted = false)
        {
            try
            {
                var university = await _universityRepository.GetAsync(u => 
                    u.UniversityUuid == universityUuid && 
                    u.IsDeleted == isDeleted);

                if (university == null)
                {
                    return new BuisnessLogicErrorDataResult<SelectUniversityDto>("Üniversite bulunamadı", 404);
                }

                // Update properties
                university.UniversityName = universityName.Trim().ToUpper();
                university.UniversityCode = universityCode.Trim().ToUpper();
                university.RegionId = regionId;
                university.UniversityTypeId = universityTypeId;
                university.EstablishedYear = establishedYear;
                university.WebsiteUrl = websiteUrl?.Trim().ToLower();
                university.IsActive = isActive;
                university.IsDeleted = isDeleted;

                var updatedUniversity = await _universityRepository.UpdateAsync(university);

                var universityDto = _mapper.Map<SelectUniversityDto>(updatedUniversity);

                _logger.LogInformation("Üniversite başarıyla güncellendi. ID: {UniversityId}", universityUuid);

                return new BuisnessLogicSuccessDataResult<SelectUniversityDto>(
                    universityDto, 200, "Üniversite başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite güncelleme sırasında veritabanı hatası. ID: {UniversityId}", 
                    universityUuid);
                return new BuisnessLogicErrorDataResult<SelectUniversityDto>(
                    "Üniversite güncellenirken veritabanı hatası oluştu", 500);
            }
        }

        /// <summary>
        /// Üniversiteyi siler (soft delete)
        /// </summary>
        public async Task<IBuisnessLogicResult> DeleteUniversityByUuidAsync(Guid universityUuid)
        {
            try
            {
                var university = await _universityRepository.GetAsync(u => 
                    u.UniversityUuid == universityUuid && 
                    u.IsDeleted == false);

                if (university == null)
                {
                    return new BuisnessLogicErrorResult("Üniversite bulunamadı", 404);
                }

                university.IsDeleted = true;
                university.DeletedAt = DateTime.UtcNow;

                await _universityRepository.UpdateAsync(university);

                _logger.LogInformation("Üniversite başarıyla silindi (Soft). ID: {UniversityId}", universityUuid);

                return new BuisnessLogicSuccessResult("Üniversite başarıyla silindi", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite silme sırasında veritabanı hatası. ID: {UniversityId}", 
                    universityUuid);
                return new BuisnessLogicErrorResult("Üniversite silinirken veritabanı hatası oluştu", 500);
            }
        }

        public async Task<IBuisnessLogicResult> HardDeleteUniversityByUuidAsync(Guid universityUuid)
        {
            try
            {
                University? university = await _universityRepository.GetAsync(u =>
                    u.UniversityUuid == universityUuid);

                if (university == null)
                {
                    return new BuisnessLogicErrorResult("Üniversite bulunamadı (Hard).", 404);
                }

                await _universityRepository.DeleteAsync(university);

                _logger.LogInformation("Üniversite başarıyla silindi (Hard). ID: {UniversityId}", universityUuid);

                return new BuisnessLogicSuccessResult("Üniversite başarıyla silindi", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üniversite silme (Hard) sırasında veritabanı hatası. ID: {UniversityId}",
                    universityUuid);
                return new BuisnessLogicErrorResult("Üniversite silinirken (Hard) veritabanı hatası oluştu", 500);
            }
        }

        #endregion
    }
}