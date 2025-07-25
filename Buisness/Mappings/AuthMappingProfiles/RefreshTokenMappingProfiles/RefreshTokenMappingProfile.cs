using AutoMapper;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.RefreshTokenMappingProfiles
{
    public class RefreshTokenMappingProfile : Profile
    {
        public RefreshTokenMappingProfile()
        {
            CreateMap<RefreshTokenCommand, RefreshTokenRequestDto>()
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanRefreshToken(src.RefreshToken)))

                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanAccessToken(src.AccessToken)))


                // Internal properties
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())

                .ForMember(dest => dest.SessionUuid, opt => opt.Ignore());
        }
    }
}
