using Core.Database.Base;
using DataAccess.Abstract;
using DataAccess.Database.Context;
using Domain.Entities.MainEntities;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRegionDal : EfGenericRepositoryBase<Region, UnifeContext>, IRegionRepository
    {
        public EfRegionDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<bool> IsRegionCodeAlpha2ExistsAsync(string regionCodeAlpha2)
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var count = await context.Regions
                    .Where(r => r.RegionCodeAlpha2 == regionCodeAlpha2)
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                // Fallback: FirstOrDefault kullan
                try
                {
                    using var context = _connectionFactory.CreateContext();
                    var region = await context.Regions
                        .FirstOrDefaultAsync(r => r.RegionCodeAlpha2 == regionCodeAlpha2);
                    return region != null;
                }
                catch
                {
                    return false; // En son durumda false döndür
                }
            }
        }

        public async Task<bool> IsRegionCodeAlpha3ExistsAsync(string regionCodeAlpha3)
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var count = await context.Regions
                    .Where(r => r.RegionCodeAlpha3 == regionCodeAlpha3)
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                try
                {
                    using var context = _connectionFactory.CreateContext();
                    var region = await context.Regions
                        .FirstOrDefaultAsync(r => r.RegionCodeAlpha3 == regionCodeAlpha3);
                    return region != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> IsRegionCodeNumericExistsAsync(string regionCodeNumeric)
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var count = await context.Regions
                    .Where(r => r.RegionCodeNumeric == regionCodeNumeric)
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                try
                {
                    using var context = _connectionFactory.CreateContext();
                    var region = await context.Regions
                        .FirstOrDefaultAsync(r => r.RegionCodeNumeric == regionCodeNumeric);
                    return region != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> IsRegionIdExistsAsync(int regionId)
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var count = await context.Regions
                    .Where(r => r.RegionId == regionId)
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                // Fallback: FirstOrDefault kullan
                try
                {
                    using var context = _connectionFactory.CreateContext();
                    var region = await context.Regions
                        .FirstOrDefaultAsync(r => r.RegionId == regionId);
                    return region != null;
                }
                catch
                {
                    return false; // En son durumda false döndür
                }
            }
        }

        public async Task<bool> IsRegionNameExistsAsync(string regionName)
        {
            try
            {
                using var context = _connectionFactory.CreateContext();
                var count = await context.Regions
                    .Where(r => r.RegionName == regionName)
                    .CountAsync();
                return count > 0;
            }
            catch
            {
                try
                {
                    using var context = _connectionFactory.CreateContext();
                    var region = await context.Regions
                        .FirstOrDefaultAsync(r => r.RegionName == regionName);
                    return region != null;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}