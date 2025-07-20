using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Repositories.Abstract.Base;

namespace DataAccess.Abstract.Repositories.AuthorizationModuleRepositories
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<Staff?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Staff>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
    }
}
