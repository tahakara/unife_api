using AutoMapper;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Concrete.ServiceManager;
using DataAccess.Abstract.Repositories.AuthorizationModuleRepositories;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices
{
    public class StudentService : ServiceManagerBase, IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(
            IStudentRepository studentRepository,
            IMapper mapper,
            ILogger<StudentService> logger,
            IServiceProvider serviceProvider)
            : base(logger, serviceProvider)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Student> CreateNewStudentAsync(Student student)
        {
            return await _studentRepository.AddAsync(student);
        }

        public async Task<IEnumerable<Student>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            return await _studentRepository.GetByEmailAsync(email, isDeleted);
        }

        public async Task<IEnumerable<Student>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            return await _studentRepository.GetByPhoneNumberAndEmailAsync(phoneCountryCode, phoneNumber, email, isDeleted);
        }

        public async Task<IEnumerable<Student>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _studentRepository.GetByPhoneNumberAsync(phoneCountryCode, phoneNumber, isDeleted);
        }

        public async Task<Student?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            return await _studentRepository.GetByUuidAsync(uuid, isDeleted);
        }

        public async Task<Student?> GetStudentByEmailAsync(string email, bool isDeleted = false)
        {
            return await _studentRepository.GetAsync(student => student.Email == email && student.IsDeleted == isDeleted);
        }

        public async Task<Student?> GetStudentByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _studentRepository.GetAsync(student => student.PhoneCountryCode == phoneCountryCode 
                                                             && student.PhoneNumber == phoneNumber 
                                                             && student.IsDeleted == isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            return await _studentRepository.IsEmailExistsAsync(email, isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _studentRepository.IsPhoneNumberExistsAsync(phoneCountryCode, phoneNumber, isDeleted);
        }
    }
}
