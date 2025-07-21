using AutoMapper;
using Buisness.DTOs.AuthDtos.SignUpDtos.Request;
using Buisness.DTOs.AuthDtos.SignUpDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.SignUpMappingProfiles
{
    public sealed class SignUpMappingProfile : Profile
    {
        public SignUpMappingProfile()
        {
            // SignUpCommand -> SignUpRequestDto Mapping
            CreateMap<SignUpCommand, SignUpRequestDto>()
            .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
            .ForMember(dest => dest.UniversityUuid, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName == null ? null : src.FirstName.Trim()))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName == null ? null : src.MiddleName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName == null ? null : src.LastName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email == null ? null : src.Email.Trim()))
            .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src =>
                src.PhoneCountryCode == null
                    ? null
                    : new string(src.PhoneCountryCode.Where(char.IsDigit).ToArray())))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                src.PhoneNumber == null
                    ? null
                    : new string(src.PhoneNumber.Where(char.IsDigit).ToArray())))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));


            // SignUpRequestDto -> Admin, Staff, Student Mapping
            CreateMap<SignUpRequestDto, Admin>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))

                .ForMember(dest => dest.AdminUuid, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore())
                .ForMember(dest => dest.IsPhoneNumberVerified, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.IsSystemPermission, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())

                .ForMember(dest => dest.University, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityEvents, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedRoles, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.AdminRoles, opt => opt.Ignore())
                .ForMember(dest => dest.AdminPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.SuspendedStaffSuspensions, opt => opt.Ignore())
                .ForMember(dest => dest.SuspendedStudentSuspensions, opt => opt.Ignore());


            CreateMap<SignUpRequestDto, Staff>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))

                .ForMember(dest => dest.StaffUuid, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore())
                .ForMember(dest => dest.IsPhoneNumberVerified, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())

                .ForMember(dest => dest.University, opt => opt.Ignore())
                .ForMember(dest => dest.StaffSuspensions, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityEvents, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedRoles, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.SuspendedStaffSuspensions, opt => opt.Ignore())
                .ForMember(dest => dest.SuspendedStudentSuspensions, opt => opt.Ignore())
                .ForMember(dest => dest.StaffRoles, opt => opt.Ignore())
                .ForMember(dest => dest.StaffPermissions, opt => opt.Ignore());


            CreateMap<SignUpRequestDto, Student>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))

                .ForMember(dest => dest.StudentUuid, opt => opt.Ignore())
                .ForMember(dest => dest.IsEmailVerified, opt => opt.Ignore())
                .ForMember(dest => dest.IsPhoneNumberVerified, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())

                .ForMember(dest => dest.University, opt => opt.Ignore())
                .ForMember(dest => dest.StudentSuspensions, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityEvents, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedRoles, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.StudentRoles, opt => opt.Ignore())
                .ForMember(dest => dest.StudentPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.SuspendedStudentSuspensions, opt => opt.Ignore());




            // Admin, Staff, Student Entity -> SignUpResponseDto Mapping
            CreateMap<Admin, SignUpResponseDto>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Staff, SignUpResponseDto>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Student, SignUpResponseDto>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.MapFrom(src => src.UniversityUuid))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}
