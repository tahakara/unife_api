using Core.Database.Base;
using DataAccess.Abstract;
using DataAccess.Context;
using Domain.Entities;
using Domain.Entities.Base.Concrete;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUniversityDal : EfGenericRepositoryBase<University, UnifeContext>, IUniversityRepository
    {
        public EfUniversityDal(IDbConnectionFactory<UnifeContext> connectionFactory) 
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<University>> GetByEstablishedYearAsync(int year)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Universities
                .Where(u => u.EstablishedYear == year && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<University>> GetByEstablishedYearAsync(int minYear, int maxYear)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Universities
                .Where(u => u.EstablishedYear >= minYear && u.EstablishedYear <= maxYear && !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<University?> GetByUuidAsync(Guid uuid)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Universities
                .FirstOrDefaultAsync(u => u.UniversityUuid == uuid && !u.IsDeleted);
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Universities
                .AnyAsync(u => u.UniversityCode == code && !u.IsDeleted);
        }

        public async Task<bool> IsWebsiteExistsAsync(string websiteUrl)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Universities
                .AnyAsync(u => u.WebsiteUrl == websiteUrl && !u.IsDeleted);
        }
    }
}