using AutoMapper;
using Buisness.DTOs.UniversityDtos;
using Domain.Entities.Base.Concrete;

namespace Buisness.Mappings
{
    public class UniversityMappingProfile : Profile
    {
        public UniversityMappingProfile()
        {
            #region SelectUniversityDto
            // Entity to DTO
            CreateMap<University, SelectUniversityDto>()
                //.ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.RegionName : null))
                //.ForMember(dest => dest.UniversityTypeName, opt => opt.MapFrom(src => src.UniversityType != null ? src.UniversityType.TypeName : null))
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => src.EstablishedYear)) // Map year directly
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl ?? string.Empty)); // Ensure URL is not null

            CreateMap<SelectUniversityDto, University>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
                .ForMember(dest => dest.UniversityTypeId, opt => opt.MapFrom(src => src.UniversityTypeId))
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => src.EstablishedYear)) // Map year directly
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl ?? string.Empty)) // Ensure URL is not null
                .ForMember(dest => dest.AddressUuid, opt => opt.Ignore()) // Address can be set later
                .ForMember(dest => dest.Region, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityType, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityCommunications, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityAddresses, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.Faculties, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityFacultyDepartments, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.Academicians, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.Rector, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.MainAddress, opt => opt.Ignore()); // Navigation property
            #endregion

            #region CreateUniversityDto
            // Create DTO to Entity
            CreateMap<CreateUniversityDto, University>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // System generates
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Region, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityType, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.AddressUuid, opt => opt.Ignore())
                .ForMember(dest => dest.MainAddress, opt => opt.Ignore()) // Main address can be set later
                .ForMember(dest => dest.UniversityCommunications, opt => opt.Ignore()) // Communications can be set later
                .ForMember(dest => dest.UniversityAddresses, opt => opt.Ignore()) // Addresses can be set later
                .ForMember(dest => dest.Faculties, opt => opt.Ignore()) // Faculties can be set later
                .ForMember(dest => dest.UniversityFacultyDepartments, opt => opt.Ignore()) // Departments can be set later
                .ForMember(dest => dest.Academicians, opt => opt.Ignore()) // Academicians can be set later
                .ForMember(dest => dest.Rector, opt => opt.Ignore()) // Rector can be set later
                .ForMember(dest => dest.UniversityTypeId, opt => opt.MapFrom(src => src.UniversityTypeId))
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => src.EstablishedYear)); // Map year directly

            CreateMap<University, CreateUniversityDto>()
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.UniversityName))
                .ForMember(dest => dest.UniversityCode, opt => opt.MapFrom(src => src.UniversityCode))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
                .ForMember(dest => dest.UniversityTypeId, opt => opt.MapFrom(src => src.UniversityTypeId))
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => src.EstablishedYear))
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            #endregion

            #region UpdateUniversityDto
            // Update DTO to Entity (for partial updates)
            CreateMap<UpdateUniversityDto, University>()
                .ForMember(dest => dest.UniversityUuid, opt => opt.Ignore()) // Cannot change
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Cannot change
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // System updates
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // Business logic handles
                .ForMember(dest => dest.Region, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.RegionId, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.UniversityType, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.AddressUuid, opt => opt.Ignore()) // Address can be set later
                .ForMember(dest => dest.MainAddress, opt => opt.Ignore()) // Main address can be set later
                .ForMember(dest => dest.UniversityCommunications, opt => opt.Ignore()) // Communications can be set later
                .ForMember(dest => dest.UniversityAddresses, opt => opt.Ignore()) // Addresses can be set later
                .ForMember(dest => dest.Faculties, opt => opt.Ignore()) // Faculties can be set later
                .ForMember(dest => dest.UniversityFacultyDepartments, opt => opt.Ignore()) // Departments can be set later
                .ForMember(dest => dest.Academicians, opt => opt.Ignore()) // Academicians can be set later
                .ForMember(dest => dest.Rector, opt => opt.Ignore()) // Rector can be set later
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.UniversityName ?? string.Empty)) // Ensure name is not null
                .ForMember(dest => dest.UniversityCode, opt => opt.MapFrom(src => src.UniversityCode ?? string.Empty)) // Ensure code is not null
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => (int?)src.EstablishedYear)) // Map year to nullable
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl ?? string.Empty)); // Ensure URL is not null

            // Entity to Update DTO (for partial updates)
            CreateMap<University, UpdateUniversityDto>()
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.UniversityName))
                .ForMember(dest => dest.UniversityCode, opt => opt.MapFrom(src => src.UniversityCode))
                .ForMember(dest => dest.EstablishedYear, opt => opt.MapFrom(src => src.EstablishedYear))
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.WebsiteUrl))
                .ForMember(dest => dest.UniversityTypeId, opt => opt.MapFrom(src => src.UniversityTypeId))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId));
            #endregion

        }
    }
}