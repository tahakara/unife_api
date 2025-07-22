using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.PsswordMappingProfiles
{
    public class ChangePasswordProfile : Profile
    {
        public ChangePasswordProfile()
        {
            CreateMap<ChangePasswordCommand, ChangePasswordRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src =>
                    src.AccessToken != null
                        ? (src.AccessToken.Trim().StartsWith("Bearer ", StringComparison.Ordinal)
                            ? src.AccessToken.Trim().Substring(7).Trim()
                            : src.AccessToken.Trim())
                        : null))
                .ForMember(dest => dest.OldPassword, opt => opt.MapFrom(src => src.OldPassword))
                .ForMember(dest => dest.NewPassword, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword))
                .ForMember(dest => dest.LogoutOtherSessions, opt => opt.MapFrom(src => src.LogoutOtherSessions));
                
        }
    }
}
