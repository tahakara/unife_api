using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.PsswordMappingProfiles
{
    public class ForgotPasswordProfile : Profile
    {
        public ForgotPasswordProfile()
        {
            CreateMap<ForgotPasswordCommand, ForgotPasswordRequestDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src => src.PhoneCountryCode))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore());
        }
    }

    public class ForgotPasswordRecoveryTokenProfile : Profile
    {
        public ForgotPasswordRecoveryTokenProfile()
        {
            CreateMap<ForgotPasswordRecoveryTokenCommand, ForgotPasswordRecoveryTokenRequestDto>()
                .ForMember(dest => dest.RecoveryToken, opt => opt.MapFrom(src => src.RecoveryToken));
        }
    }
}
