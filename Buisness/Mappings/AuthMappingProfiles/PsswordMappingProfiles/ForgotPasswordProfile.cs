using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Mappings.MappingHelpers;
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
            //Command to DTO Mapping
            CreateMap<ForgotPasswordCommand, ForgotPasswordRequestDto>()
                .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src =>
                    MappingHelper.CleanUserTypeId(src.UserTypeId)))

                .ForMember(dest => dest.RecoveryMethodId, opt => opt.MapFrom(src =>
                    MappingHelper.CleanRecoveryMethodId(src.RecoveryMethodId)))

                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                    MappingHelper.CleanEmail(src.Email)))

                .ForMember(dest => dest.PhoneCountryCode, opt => opt.MapFrom(src =>
                    MappingHelper.CleanPhoneCountryCode(src.PhoneCountryCode)))

                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    MappingHelper.CleanPhoneNumber(src.PhoneNumber)))


                // Internal Properties
                .ForMember(dest => dest.RecoveryToken, opt => opt.Ignore())

                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())

                .ForMember(dest => dest.RecoverySessionUuid, opt => opt.Ignore());
        }
    }
}
