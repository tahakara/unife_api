using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Repositories.Abstract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract.Repositories.AuthorizationModuleRepositories
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);

    }
}
