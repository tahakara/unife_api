using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ChangePasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ChangePassword;
using Buisness.Mappings.MappingHelpers;
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
                    MappingHelper.CleanAccessToken(src.AccessToken)))
                    
                .ForMember(dest => dest.OldPassword, opt => opt.MapFrom(src => src.OldPassword ?? string.Empty))

                .ForMember(dest => dest.NewPassword, opt => opt.MapFrom(src => src.NewPassword ?? string.Empty))

                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword ?? string.Empty))

                .ForMember(dest => dest.LogoutOtherSessions, opt => opt.MapFrom(src => 
                    MappingHelper.BoolOrDefaultBooleanValue(src.LogoutOtherSessions)));
                
        }
    }
}
