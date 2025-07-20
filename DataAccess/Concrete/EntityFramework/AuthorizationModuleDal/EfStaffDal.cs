using Core.Database.Base;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories;
using DataAccess.Database.Context;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.AuthorizationModuleDal
{
    public class EfStaffDal : EfGenericRepositoryBase<Staff, UnifeContext>, IStaffRepository
    {
        public EfStaffDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<Staff>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .Where(s => s.Email == email && s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .Where(s => s.PhoneCountryCode == phoneCountryCode &&
                            s.PhoneNumber == phoneNumber &&
                            s.Email == email &&
                            s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Staff>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .Where(s => s.PhoneCountryCode == phoneCountryCode &&
                            s.PhoneNumber == phoneNumber &&
                            s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<Staff?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .FirstOrDefaultAsync(s => s.StaffUuid == uuid && s.IsDeleted == isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .AnyAsync(s => s.Email == email && s.IsDeleted == isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Staff>()
                .AnyAsync(s => s.PhoneCountryCode == phoneCountryCode &&
                               s.PhoneNumber == phoneNumber &&
                               s.IsDeleted == isDeleted);
        }
    }
}
