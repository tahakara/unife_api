using AutoMapper;
using Buisness.DTOs.AuthDtos.PasswordDtos.ForgotPasswordDtos;
using Buisness.Features.CQRS.Auth.Commands.Password.ForgotPassword;
using Buisness.Mappings.MappingHelpers;
using Buisness.Validators.Common;
using Core.Enums;

namespace Buisness.Mappings.AuthMappingProfiles.PsswordMappingProfiles
{
    public class ForgotPasswordRecoveryTokenProfile : Profile
    {
        public ForgotPasswordRecoveryTokenProfile()
        {
            CreateMap<ForgotPasswordRecoveryTokenCommand, ForgotPasswordRecoveryTokenRequestDto>()
                .ForMember(dest => dest.RecoveryToken, opt => opt.MapFrom(src =>
                    MappingHelper.CleanRecoveryToken(src.RecoveryToken)))

                .ForMember(dest => dest.NewPassword, opt => opt.MapFrom(src => src.NewPassword ?? string.Empty))

                .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword ?? string.Empty))


                // Internal properties 
                .ForMember(dest => dest.UserTypeId, opt => opt.Ignore())

                .ForMember(dest => dest.UserUuid, opt => opt.Ignore())

                .ForMember(dest => dest.RecoverySessionUuid, opt => opt.Ignore());
        }
    }
}
