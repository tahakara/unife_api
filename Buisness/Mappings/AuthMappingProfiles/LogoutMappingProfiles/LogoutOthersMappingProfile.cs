using AutoMapper;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.LogoutMappingProfiles
{
    public class LogoutOthersMappingProfile : Profile
    {
        public LogoutOthersMappingProfile()
        {
            CreateMap<LogoutOthersRequestDto, LogoutOthersCommand>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanAccessToken(src.AccessToken)));

            CreateMap<LogoutOthersCommand, LogoutOthersRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanAccessToken(src.AccessToken)));
        }
    }
}
