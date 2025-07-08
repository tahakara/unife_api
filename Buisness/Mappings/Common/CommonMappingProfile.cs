using AutoMapper;
using Buisness.DTOs.Common;
using Buisness.Validators.FluentValidation.Validators.Common.Response;
using Buisness.Validators.FluentValidation.Validators.Common.Request;
using Buisness.DTOs.UniversityDtos;

namespace Buisness.Mappings.Common
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            // PaginationRequest to PagedResponse mapping - sadece sayfa bilgileri
            // Data property'si ignore edilir çünkü PaginationRequest'te yok
            CreateMap(typeof(PaginationRequest), typeof(PagedResponse<>))
                .ForMember("Data", opt => opt.Ignore()) // PaginationRequest'te Data yok
                .ForMember("TotalCount", opt => opt.MapFrom("TotalCount"))
                .ForMember("CurrentPage", opt => opt.MapFrom("CurrentPage"))
                .ForMember("PageSize", opt => opt.MapFrom("PageSize"));

            // PagedResponse to PaginationResponse mapping
            CreateMap(typeof(PagedResponse<>), typeof(PaginationResponse<>))
                .ForMember("Data", opt => opt.MapFrom("Data"))
                .ForMember("TotalCount", opt => opt.MapFrom("TotalCount"))
                .ForMember("CurrentPage", opt => opt.MapFrom("CurrentPage"))
                .ForMember("PageSize", opt => opt.MapFrom("PageSize"));

            // PaginationResponse to PagedResponse mapping (tersine)
            CreateMap(typeof(PaginationResponse<>), typeof(PagedResponse<>))
                .ForMember("Data", opt => opt.MapFrom("Data"))
                .ForMember("TotalCount", opt => opt.MapFrom("TotalCount"))
                .ForMember("CurrentPage", opt => opt.MapFrom("CurrentPage"))
                .ForMember("PageSize", opt => opt.MapFrom("PageSize"));

            // List to PagedResponse mapping - Daha kullanışlı
            CreateMap<IEnumerable<SelectUniversityDto>, PagedResponse<SelectUniversityDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.TotalCount, opt => opt.Ignore()) // Manuel set edilecek
                .ForMember(dest => dest.CurrentPage, opt => opt.Ignore()) // Manuel set edilecek
                .ForMember(dest => dest.PageSize, opt => opt.Ignore()); // Manuel set edilecek

            // List to PaginationResponse mapping
            CreateMap<IEnumerable<SelectUniversityDto>, PaginationResponse<SelectUniversityDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.TotalCount, opt => opt.Ignore()) // Manuel set edilecek
                .ForMember(dest => dest.CurrentPage, opt => opt.Ignore()) // Manuel set edilecek
                .ForMember(dest => dest.PageSize, opt => opt.Ignore()); // Manuel set edilecek
        }
    }
}