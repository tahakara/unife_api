using Core.Database.Base;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories;
using DataAccess.Abstract.Repositories.UniversityModuleRepositories;
using DataAccess.Database.Context;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Entities.MainEntities.UniversityModul;
using Domain.Repositories.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.AuthorizationModuleDal
{
    public class EfAdminDal : EfGenericRepositoryBase<Admin, UnifeContext>, IAdminRepository
    {
        public EfAdminDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<Admin>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .Where(a => a.Email == email && a.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Admin>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .Where(a => a.PhoneCountryCode == phoneCountryCode 
                            && a.PhoneNumber == phoneNumber 
                            && a.Email == email 
                            && a.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Admin>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .Where(a => a.PhoneCountryCode == phoneCountryCode 
                            && a.PhoneNumber == phoneNumber 
                            && a.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<Admin?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .FirstOrDefaultAsync(a => a.AdminUuid == uuid && a.IsDeleted == isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .AnyAsync(a => a.Email == email && a.IsDeleted == isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            var context = await _connectionFactory.CreateContextAsync();
            return await context.Admins
                .AnyAsync(a => a.PhoneCountryCode == phoneCountryCode 
                               && a.PhoneNumber == phoneNumber 
                               && a.IsDeleted == isDeleted);
        }
    }
}
