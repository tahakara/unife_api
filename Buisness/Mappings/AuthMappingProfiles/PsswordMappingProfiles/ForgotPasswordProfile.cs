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
                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src =>
                    src.PhoneCountryCode == null
                        ? null
                        : new string(src.PhoneCountryCode.Where(char.IsDigit).ToArray())))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    src.PhoneNumber == null
                        ? null
                        : new string(src.PhoneNumber.Where(char.IsDigit).ToArray())))
                .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore());
        }
    }
}
