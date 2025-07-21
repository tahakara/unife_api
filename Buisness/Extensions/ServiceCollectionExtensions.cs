using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices;
using Buisness.Abstract.ServicesBase.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Behaviors;
using Buisness.DTOs.AuthDtos;
using Buisness.DTOs.AuthDtos.LogoutDtos.RequestDtos;
using Buisness.DTOs.AuthDtos.RefreshDtos;
using Buisness.Features.CQRS.Auth.Commands.Logout.Logout;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutAll;
using Buisness.Features.CQRS.Auth.Commands.Logout.LogoutOthers;
using Buisness.Features.CQRS.Auth.Commands.RefreshToken;
using Buisness.Features.CQRS.Auth.Commands.SignIn;
using Buisness.Features.CQRS.Auth.Commands.SignUp;
using Buisness.Features.CQRS.Auth.Commands.Verify.VerifyOTP;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;
using Buisness.Mappings;
using Buisness.Mappings.AuthMappingProfiles.LogoutMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.RefreshTokenMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.ResendSignInOTPProfiles;
using Buisness.Mappings.AuthMappingProfiles.SignInMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.SignUpMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.VerifyMappingProfiles;
using Buisness.Mappings.Common;
using Buisness.Services.EntityRepositoryServices;
using Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices;
using Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Services.UtilityServices.Base.EmailServices;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Buisness.Services.UtilityServices.EmailServices;
using Buisness.Services.UtilityServices.ObjectStorageServices;
using Buisness.Validators.FluentValidation.Validators.AuthValidators;
using Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.LogoutValidators;
using Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.RefreshTokenValidators;
using Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.SignInValidators;
using Buisness.Validators.FluentValidation.Validators.AuthValidators.Request.VerifyOTPValidators;
using Buisness.Validators.FluentValidation.Validators.University.Request;
using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Core.Security.JWT.Extensions;
using Core.Utilities.BuisnessLogic.Base;
using Core.Utilities.OTPUtilities;
using Core.Utilities.OTPUtilities.Base;
using Core.Utilities.PasswordUtilities;
using Core.Utilities.PasswordUtilities.Base;
using Core.Database;
using Core.ObjectStorage.Redis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Buisness.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            // MediatR Registration
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });


            // Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<AccessTokenDto> , AccessTokenDtoValidator>();
            services.AddScoped<IValidator<LogoutCommand>, LogoutRequestDtoValidator>();
            services.AddScoped<IValidator<LogoutAllCommand>, LogoutAllRequestDtoValidator>();
            services.AddScoped<IValidator<LogoutOthersCommand>, LogoutOthersRequestDtoValidator>();
            services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenRequestDtoValidator>();

            services.AddScoped<IValidator<SignUpCommand>, SignUpRequestDtoValidator>();
            services.AddScoped<IValidator<SignInCommand>, SignInRequestDtoValidator>();
            services.AddScoped<IValidator<VerifyOTPCommand>, VerifyOTPRequestDtoValidator>();

            services.AddScoped<IValidator<CreateUniversityCommand>, CreateUniversityDtoValidator>();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<LogoutMappingProfile>();
                cfg.AddProfile<LogoutAllMappingProfile>();
                cfg.AddProfile<LogoutOthersMappingProfile>();
                cfg.AddProfile<SignUpMappingProfile>();
                cfg.AddProfile<SignInMappingProfile>();
                cfg.AddProfile<ResendSignInOTPProfile>();
                cfg.AddProfile<VerifyOTPMappingProfile>();

                cfg.AddProfile<RefreshTokenMappingProfile>();
                cfg.AddProfile<UniversityMappingProfile>();
                cfg.AddProfile<CommonMappingProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            // *** UNIFE SERVICES - Sadeleştirilmiş Yapı ***
            services.AddScoped<ICacheService, UnifeCacheService>();
            services.AddScoped<ISessionJwtService, SessionJwtService>();
            services.AddSingleton<IPasswordUtility, PasswordUtility>();
            services.AddSingleton<IOTPUtilitiy, OTPUtilitiy>();

            // Business Services
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISecurityEventService, SecurityEventService>();
            services.AddScoped<ISecurityEventTypeService, SecurityEventTypeService>();

            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUniversityTypeService, UniversityTypeService>();
            services.AddScoped<IRegionService, RegionService>();

            // Business Logic Helper
            services.AddScoped<IAuthBuisnessLogicHelper, AuthBuisnessLogicHelper>();
            services.AddScoped<IUniversityBuisnessLogicHelper, UniversityBusinessLogicHelper>();

            // Database Management Services
            services.AddScoped<UnifeConnectionFactory>();

            // JWT Core services
            services.AddJwtCore(configuration);

            services.AddScoped<IEmailService, EmailService>();

            // Verification Code Service
            services.AddScoped<IOTPCodeService, VerificationCodeService>();
            services.AddKeyedScoped<IObjectStorageConnectionFactory, GenericRedisConnectionFactory>("verification", (sp, key) =>
    new GenericRedisConnectionFactory(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<GenericRedisConnectionFactory>>(),
        RedisStorageType.VerificationCode));

            return services;
        }
    }
}