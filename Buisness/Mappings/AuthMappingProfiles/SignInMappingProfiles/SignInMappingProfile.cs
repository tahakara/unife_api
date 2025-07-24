using AutoMapper;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Domain.Entities.MainEntities.AuthorizationModuleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.SignInMappingProfiles
{
    public class SignInMappingProfile : Profile
    {
        public SignInMappingProfile()
        {
            CreateMap<SignInCommand, SignInRequestDto>()
                .ForMember(x => x.UserTypeId, opt => opt.MapFrom(src => src.UserTypeId))
                //.ForMember(x => x.UserUuid, opt => opt.MapFrom(src => src.UserUuid))
                //.ForMember(x => x.SessionUuid, opt => opt.MapFrom(src => src.SessionUuid))
                .ForMember(x => x.UserUuid, opt => opt.Ignore())
                .ForMember(x => x.SessionUuid, opt => opt.Ignore())
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
