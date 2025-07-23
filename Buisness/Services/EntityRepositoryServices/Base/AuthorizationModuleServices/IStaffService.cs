using Buisness.Services.EntityRepositoryServices.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices
{
    public interface IStaffService : IServiceManagerBase
    {
        Task<Staff> UpdateStaffAsync(Staff staff);
        Task<Staff> CreateNewStaffAsync(Staff staff);
        Task<Staff?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<Staff?> GetStaffByEmailAsync(string email, bool isDeleted = false);
        Task<Staff?> GetStaffByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
    }
}
