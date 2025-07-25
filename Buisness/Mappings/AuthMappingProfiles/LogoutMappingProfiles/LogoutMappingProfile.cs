using AutoMapper;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.LogoutMappingProfiles
{
    public class LogoutMappingProfile : Profile
    {
        public LogoutMappingProfile()
        {
            CreateMap<LogoutRequestDto, LogoutCommand>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => 
                    MappingHelper.CleanAccessToken(src.AccessToken)));

            CreateMap<LogoutCommand, LogoutRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanAccessToken(src.AccessToken)));
        }
    }
}
