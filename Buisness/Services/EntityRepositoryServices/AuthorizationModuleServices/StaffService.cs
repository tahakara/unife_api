using AutoMapper;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Concrete.ServiceManager;
using Core.Abstract.Repositories.AuthorizationModuleRepositories;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using Microsoft.Extensions.Logging;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices
{
    public class StaffService : ServiceManagerBase, IStaffService
    {
        private readonly IStaffRepository _staffRepository;
        public StaffService(
            IStaffRepository staffRepository,
            IMapper mapper,
            ILogger<StaffService> logger, 
            IServiceProvider serviceProvider) 
            : base(mapper, logger, serviceProvider)
        {
            _staffRepository = staffRepository;
        }

        public async Task<Staff> CreateNewStaffAsync(Staff staff)
        {
            return await _staffRepository.AddAsync(staff);
        }

        public async Task<IEnumerable<Staff>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            return await _staffRepository.GetByEmailAsync(email, isDeleted);
        }

        public async Task<IEnumerable<Staff>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            return await _staffRepository.GetByPhoneNumberAndEmailAsync(phoneCountryCode, phoneNumber, email, isDeleted);
        }

        public async Task<IEnumerable<Staff>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _staffRepository.GetByPhoneNumberAsync(phoneCountryCode, phoneNumber, isDeleted);
        }

        public async Task<Staff?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            return await _staffRepository.GetByUuidAsync(uuid, isDeleted);
        }

        public async Task<Staff?> GetStaffByEmailAsync(string email, bool isDeleted = false)
        {
            return await _staffRepository.GetAsync(staff => staff.Email == email && staff.IsDeleted == isDeleted);
        }

        public async Task<Staff?> GetStaffByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _staffRepository.GetAsync(staff => staff.PhoneCountryCode == phoneCountryCode 
                                                            && staff.PhoneNumber == phoneNumber 
                                                            && staff.IsDeleted == isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            return await _staffRepository.IsEmailExistsAsync(email, isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _staffRepository.IsPhoneNumberExistsAsync(phoneCountryCode, phoneNumber, isDeleted);
        }
    }
}
