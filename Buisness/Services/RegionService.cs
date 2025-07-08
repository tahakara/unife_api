using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Concrete.ServiceManager;
using DataAccess.Abstract;
using Microsoft.Extensions.Logging;

namespace Buisness.Services
{
    public class RegionService : ServiceManagerBase, IRegionService
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        public RegionService(
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionService> logger,
            IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        public async Task<bool> IsRegionCodeAlpha2ExistsAsync(string regionCodeAlpha2)
        {
            return await _regionRepository.IsRegionCodeAlpha2ExistsAsync(regionCodeAlpha2);
        }

        public Task<bool> IsRegionCodeAlpha3ExistsAsync(string regionCodeAlpha3)
        {
            return _regionRepository.IsRegionCodeAlpha3ExistsAsync(regionCodeAlpha3);
        }

        public Task<bool> IsRegionCodeNumericExistsAsync(string regionCodeNumeric)
        {
            return _regionRepository.IsRegionCodeNumericExistsAsync(regionCodeNumeric);
        }

        public async Task<bool> IsRegionIdExistsAsync(int regionId)
        {
            return await _regionRepository.IsRegionIdExistsAsync(regionId);
        }

        public Task<bool> IsRegionNameExistsAsync(string regionName)
        {
            return _regionRepository.IsRegionNameExistsAsync(
                regionName.ToUpper());
        }
    }
}