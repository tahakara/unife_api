using AutoMapper;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.LogoutMappingProfiles
{
    public class LogoutAllMappingProfile : Profile
    {
        public LogoutAllMappingProfile()
        {
            // Command to DTO mapping
            CreateMap<LogoutAllCommand, LogoutAllRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => 
                    MappingHelper.CleanAccessToken(src.AccessToken)));

            // DTO to Command mapping
            CreateMap<LogoutAllRequestDto, LogoutAllCommand>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => 
                    MappingHelper.CleanAccessToken(src.AccessToken)));

        }
    }
}
