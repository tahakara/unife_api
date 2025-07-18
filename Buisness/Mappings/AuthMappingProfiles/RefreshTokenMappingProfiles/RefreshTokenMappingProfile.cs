using AutoMapper;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
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
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken != null ? src.RefreshToken.Trim() : null))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    src.AccessToken != null
                        ? (src.AccessToken.Trim().StartsWith("Bearer ", StringComparison.Ordinal)
                            ? src.AccessToken.Trim().Substring(7).Trim()
                            : src.AccessToken.Trim())
                        : null))
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())
                .ForMember(dest => dest.SessionUuid, opt => opt.Ignore());

            CreateMap<RefreshTokenRequestDto, RefreshTokenCommand>()
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken != null ? src.RefreshToken.Trim() : null))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken != null ? src.AccessToken.Trim() : null))
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())
                .ForMember(dest => dest.SessionUuid, opt => opt.Ignore());

            CreateMap<RefreshTokenResponseDto, RefreshTokenCommand>()
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken != null ? src.RefreshToken.Trim() : null))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken != null ? src.AccessToken.Trim() : null))
                .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => src.UserUuid != null ? src.UserUuid.Trim() : null))
                .ForMember(dest => dest.SessionUuid, opt => opt.MapFrom(src => src.SessionUuid != null ? src.SessionUuid.Trim() : null));

            CreateMap<RefreshTokenCommand, RefreshTokenResponseDto>()
                .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken != null ? src.RefreshToken.Trim() : null))
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    src.AccessToken != null
                        ? (src.AccessToken.Trim().StartsWith("Bearer ", StringComparison.Ordinal)
                            ? src.AccessToken.Trim().Substring(7).Trim()
                            : src.AccessToken.Trim())
                        : null))
                .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => src.UserUuid != null ? src.UserUuid.Trim() : null))
                .ForMember(dest => dest.SessionUuid, opt => opt.MapFrom(src => src.SessionUuid != null ? src.SessionUuid.Trim() : null));
        }
    }
}
