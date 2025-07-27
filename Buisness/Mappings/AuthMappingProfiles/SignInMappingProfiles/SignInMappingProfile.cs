using AutoMapper;
using Buisness.DTOs.AuthDtos.SignInDtos.Request;
using Buisness.DTOs.AuthDtos.SignInDtos.Response;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Mappings.MappingHelpers;
using Core.Utilities.OTPUtilities;
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
            // Command to DTO mapping
            CreateMap<SignInCommand, SignInRequestDto>()
                .ForMember(x => x.UserTypeId, opt => opt.MapFrom(src =>
                    MappingHelper.CleanUserTypeId(src.UserTypeId)))

                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                    MappingHelper.CleanEmail(src.Email)))

                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src =>
                    MappingHelper.CleanPhoneCountryCode(src.PhoneCountryCode)))

                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    MappingHelper.CleanPhoneNumber(src.PhoneNumber)))

                .ForMember(x => x.Password, opt => opt.MapFrom(src =>
                    src.Password ?? string.Empty))

                // Internal properties
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())

                .ForMember(dest => dest.SessionUuid, opt => opt.Ignore())

                .ForMember(dest => dest.OtpTypeId, opt => opt.Ignore())

                .ForMember(dest => dest.OtpCode, opt => opt.Ignore());
        }
    }
}
