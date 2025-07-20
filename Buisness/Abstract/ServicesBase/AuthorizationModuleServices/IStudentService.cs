using Buisness.Abstract.ServicesBase.Base;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;

namespace Buisness.Abstract.ServicesBase.AuthorizationModuleServices
{
    public interface IStudentService : IServiceManagerBase
    {
        Task<Student> CreateNewStudentAsync(Student student);
        Task<Student?> GetByUuidAsync(Guid uuid, bool isDeleted = false);
        Task<Student?> GetStudentByEmailAsync(string email, bool isDeleted = false);
        Task<Student?> GetStudentByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByEmailAsync(string email, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
        Task<IEnumerable<Student>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false);

        Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false);
        Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false);
    }
}
