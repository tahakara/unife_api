using Core.Database.Base;
using Core.Database.Context;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Repositories.Concrete.EntityFramework;
using Core.Abstract.Repositories.AuthorizationModuleRepositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Concrete.EntityFramework.AuthorizationModuleDal
{
    public class EfStudentDal : EfGenericRepositoryBase<Student, UnifeContext>, IStudentRepository
    {
        public EfStudentDal(IDbConnectionFactory<UnifeContext> connectionFactory)
            : base(connectionFactory)
        {
        }

        public async Task<IEnumerable<Student>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Student>()
                .Where(s => s.Email == email && s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Student>()
                .Where(s => s.PhoneCountryCode == phoneCountryCode &&
                            s.PhoneNumber == phoneNumber &&
                            s.Email == email &&
                            s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            using var context = _connectionFactory.CreateContextAsync();
            return await context.Result.Set<Student>()
                .Where(s => s.PhoneCountryCode == phoneCountryCode &&
                            s.PhoneNumber == phoneNumber &&
                            s.IsDeleted == isDeleted)
                .ToListAsync();
        }

        public async Task<Student?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Student>()
                .FirstOrDefaultAsync(s => s.StudentUuid == uuid && s.IsDeleted == isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Student>()
                .AnyAsync(s => s.Email == email && s.IsDeleted == isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            using var context = await _connectionFactory.CreateContextAsync();
            return await context.Set<Student>()
                .AnyAsync(s => s.PhoneCountryCode == phoneCountryCode &&
                               s.PhoneNumber == phoneNumber &&
                               s.IsDeleted == isDeleted);
        }
    }
}
