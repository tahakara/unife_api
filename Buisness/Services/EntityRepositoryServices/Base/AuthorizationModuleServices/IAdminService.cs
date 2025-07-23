using Buisness.Services.EntityRepositoryServices.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices
{
    public interface IAdminService : IServiceManagerBase
    {
        Task<Admin> UpdateAdminAsync(Admin admin);
        Task<Admin> CreateNewAdminAsync(Admin admin);
        Task<Admin?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<Admin?> GetAdminByEmailAsync(string email, bool isDeleted = false);
        Task<Admin?> GetAdminByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Admin>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
    }
}
