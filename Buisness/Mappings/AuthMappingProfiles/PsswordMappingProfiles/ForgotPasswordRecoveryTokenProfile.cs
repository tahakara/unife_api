using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;

namespace Buisness.Mappings.AuthMappingProfiles.PsswordMappingProfiles
{
    public class ForgotPasswordRecoveryTokenProfile : Profile
    {
        public ForgotPasswordRecoveryTokenProfile()
        {
            CreateMap<ForgotPasswordRecoveryTokenCommand, ForgotPasswordRecoveryTokenRequestDto>()
                .ForMember(dest => dest.RecoveryToken, opt => opt.MapFrom(src => src.RecoveryToken))
                .ForMember(dest => dest.NewPassword, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword));

        }
    }
}
