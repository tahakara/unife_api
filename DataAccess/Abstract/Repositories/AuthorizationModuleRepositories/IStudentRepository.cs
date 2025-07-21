using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Domain.Repositories.Abstract.Base;

namespace Core.Abstract.Repositories.AuthorizationModuleRepositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
    }
}
