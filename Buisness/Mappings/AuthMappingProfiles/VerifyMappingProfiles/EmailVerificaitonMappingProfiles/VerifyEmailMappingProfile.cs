using AutoMapper;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail;
using Buisness.Mappings.MappingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Mappings.AuthMappingProfiles.VerifyMappingProfiles.EmailVerificaitonMappingProfiles
{
    public class VerifyEmailMappingProfile : Profile
    {
        public VerifyEmailMappingProfile()
        {
            CreateMap<VerifyEmailCommand, VerifyEmailRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => 
                    MappingHelper.CleanAccessToken(src.AccessToken)));
        }
    }
}
