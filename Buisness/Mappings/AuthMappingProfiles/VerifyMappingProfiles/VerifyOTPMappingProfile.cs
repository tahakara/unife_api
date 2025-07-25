using AutoMapper;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyOTPDtos;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.VerifyMappingProfiles
{
    public class VerifyOTPMappingProfile : Profile
    {
        public VerifyOTPMappingProfile()
        {
            CreateMap<VerifyOTPCommand,  VerifyOTPRequestDto>()
                .ForMember(dest => dest.UserTypeId, opt => opt.MapFrom(src => 
                    MappingHelper.CleanUserTypeId(src.UserTypeId)))

                .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => 
                    MappingHelper.CleanStringToGuid(src.UserUuid)))
                
                .ForMember(dest => dest.SessionUuid, opt => opt.MapFrom(src => 
                    MappingHelper.CleanStringToGuid(src.SessionUuid)))
                
                .ForMember(dest => dest.OtpTypeId, opt => opt.MapFrom(src => 
                    MappingHelper.CleanUserTypeId(src.UserTypeId)))
                
                .ForMember(dest => dest.OtpCode, opt => opt.MapFrom(src => 
                    MappingHelper.CleanOtpCode(src.OtpCode)));
        }
    }
}
