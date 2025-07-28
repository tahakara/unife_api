using AutoMapper;
using Buisness.DTOs.AuthDtos.VerifyDtos.VerifyEmailDtos;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyEmail;
using Buisness.Mappings.MappingHelpers;

namespace Buisness.Mappings.AuthMappingProfiles.VerifyMappingProfiles.EmailVerificaitonMappingProfiles
{
    public class SendEmailVerificationOTPMappingProfile : Profile
    {
        public SendEmailVerificationOTPMappingProfile()
        {
            CreateMap<SendEmailVerificationOTPCommand, SendEmailVerificationOTPRequestDto>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => 
                    MappingHelper.CleanAccessToken(src.AccessToken)))
                
                .ForMember(dest => dest.UserTyeId, opt => opt.Ignore())
                
                .ForMember(dest => dest.UserUuid, opt => opt.Ignore());
        }
    }
}
