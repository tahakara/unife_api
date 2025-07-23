using AutoMapper;
using Buisness.DTOs.UniversityDtos;
using Buisness.Services.EntityRepositoryServices.Base;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices;
using Core.Abstract.Repositories.AuthorizationModuleRepositories;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices
{
    public class AdminService : ServiceManagerBase, IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(
            IAdminRepository adminRepository,
            IMapper mapper,
            ILogger<AdminService> logger, 
            IServiceProvider serviceProvider) : 
            base(mapper, logger, serviceProvider)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin> UpdateAsync(Admin admin)
        {
            await LogOperationAsync("UpdateAsync", admin);
            return await _adminRepository.UpdateAsync(admin);
        }

        public Task<Admin> CreateNewAdminAsync(Admin admin)
        {
            return _adminRepository.AddAsync(admin);
        }

        public async Task<Admin?> GetAdminByEmailAsync(string email, bool isDeleted = false)
        {
            await LogOperationAsync("GetByEmailAsync", new { email, isDeleted });
            // Validate email format
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Invalid email format.");
            }
            return await _adminRepository.GetAsync(Admin => Admin.Email == email && Admin.IsDeleted == isDeleted);
        }

        public async Task<Admin?> GetAdminByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            await LogOperationAsync("GetAdminByPhoneNumberAsync", new { phoneCountryCode, phoneNumber, isDeleted });
            // Validate phone number format
            if (string.IsNullOrWhiteSpace(phoneCountryCode) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("Phone country code and number cannot be empty.");
            }

            return await _adminRepository.GetAsync(Admin => Admin.PhoneCountryCode == phoneCountryCode 
                                                            && Admin.PhoneNumber == phoneNumber 
                                                            && Admin.IsDeleted == isDeleted);
        }

        public async Task<IEnumerable<Admin>> GetByEmailAsync(string email, bool isDeleted = false)
        {
            await LogOperationAsync("GetByEmailAsync", new { email, isDeleted });

            // Validate email format
            return await _adminRepository.GetByEmailAsync(email, isDeleted);
        }

        public async Task<IEnumerable<Admin>> GetByPhoneNumberAndEmailAsync(string phoneCountryCode, string phoneNumber, string email, bool isDeleted = false)
        {
            return await _adminRepository.GetByPhoneNumberAndEmailAsync(phoneCountryCode, phoneNumber, email, isDeleted);
        }

        public async Task<IEnumerable<Admin>> GetByPhoneNumberAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _adminRepository.GetByPhoneNumberAsync(phoneCountryCode, phoneNumber, isDeleted);
        }

        public async Task<Admin?> GetByUuidAsync(Guid uuid, bool isDeleted = false)
        {
            return await _adminRepository.GetByUuidAsync(uuid, isDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email, bool isDeleted = false)
        {
            return await _adminRepository.IsEmailExistsAsync(email, isDeleted);
        }

        public async Task<bool> IsPhoneNumberExistsAsync(string phoneCountryCode, string phoneNumber, bool isDeleted = false)
        {
            return await _adminRepository.IsPhoneNumberExistsAsync(phoneCountryCode, phoneNumber, isDeleted);
        }

        public async Task<Admin> UpdateAdminAsync(Admin admin)
        {
            return await _adminRepository.UpdateAsync(admin);
        }
    }
}
