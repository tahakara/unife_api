using AutoMapper;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.Features.CQRS.Auth.Commands.ResendSignInOTP;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.ResendSignInOTPProfiles
{
    public class ResendSignInOTPProfile : Profile
    {
        public ResendSignInOTPProfile()
        {
            CreateMap<ResendSignInOTPCommand, SignInRequestDto>()
                .ForMember(x => x.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
                .ForMember(x => x.UserUuid, opt => opt.MapFrom(src => src.UserUuid))
                .ForMember(x => x.SessionUuid, opt => opt.MapFrom(src => src.SessionUuid))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email == null ? null : src.Email.Trim()))
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src =>
                    src.PhoneCountryCode == null
                        ? null
                        : new string(src.PhoneCountryCode.Where(char.IsDigit).ToArray())))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    src.PhoneNumber == null
                        ? null
                        : new string(src.PhoneNumber.Where(char.IsDigit).ToArray())))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(x => x.OtpTypeId, opt => opt.Ignore())
                .ForMember(x => x.OtpCode, opt => opt.Ignore());
        }
    }
}
