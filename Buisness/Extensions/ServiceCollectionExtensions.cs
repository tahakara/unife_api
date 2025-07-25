using AutoMapper;
using Buisness.Behaviors;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;
using Buisness.Mappings;
using Buisness.Mappings.AuthMappingProfiles.LogoutMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.PsswordMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.RefreshTokenMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.ResendSignInOTPProfiles;
using Buisness.Mappings.AuthMappingProfiles.SignInMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.SignUpMappingProfiles;
using Buisness.Mappings.AuthMappingProfiles.VerifyMappingProfiles;
using Buisness.Mappings.Common;
using Buisness.Services.EntityRepositoryServices;
using Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices;
using Buisness.Services.EntityRepositoryServices.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices;
using Buisness.Services.EntityRepositoryServices.Base.AuthorizationModuleServices.SecurityEventServices;
using Buisness.Services.EntityRepositoryServices.Base.UniversityModuleServices;
using Buisness.Services.EntityRepositoryServices.UniversityModuleServices;
using Buisness.Services.UtilityServices.Base.EmailServices;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Buisness.Services.UtilityServices.EmailServices;
using Buisness.Services.UtilityServices.ObjectStorageServices;
using Buisness.Validators.FluentValidation.Validators.University.Request;
using Core.Database;
using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Core.ObjectStorage.Redis;
using Core.Security.JWT.Extensions;
using Core.Utilities.OTPUtilities;
using Core.Utilities.OTPUtilities.Base;
using Core.Utilities.PasswordUtilities;
using Core.Utilities.PasswordUtilities.Base;
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

            // FluentValidation Aoutomatic Registration based on Assembly IValidator<T>
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // AccessToken Carrier Validator
            //var validatorTypes = new Type[]
            //{
            //    typeof(UserUuidCarrierValidator<>),
            //    typeof(SessionUuidCarrierValidator<>),
            //    typeof(AccessTokenCarrierValidator<>),
            //    typeof(NullOrValidAccessTokenCarrierValidator<>),
            //    typeof(EmailOrPhoneCarrierValidator<>),
            //    typeof(NewPasswordCarrierValidator<>),
            //    typeof(UserTypeIdCarrierValidator<>),
            //    typeof(PasswordCarrierValidator<>),
            //    typeof(EmailCarrierValidator<>),
            //    typeof(PhoneCarrierValidator<>),
            //    typeof(FirstNameCarrierValidator<>),
            //    typeof(MiddleNameCarrierValidator<>),
            //    typeof(LastNameCarrierValidator<>),
            //    typeof(UniversityUuidOptionalCarrierValidator<>)
            //};

            //services.AddValidatorServices(validatorTypes);



            // FluentValidation Validators
            //services.AddScoped<IValidator<AccessTokenDto> , AccessTokenDtoValidator>();

            //services.AddScoped<IValidator<LogoutCommand>, LogoutCommandValidator>();
            //services.AddScoped<IValidator<LogoutAllCommand>, LogoutAllCommandValidator>();
            //services.AddScoped<IValidator<LogoutOthersCommand>, LogoutOthersCommandValidator>();
            //services.AddScoped<IValidator<SignUpCommand>, SignUpCommandValidator>();
            //services.AddScoped<IValidator<SignInCommand>, SignInCommandValidator>();
            //services.AddScoped<IValidator<ResendSignInOTPCommand>, SignInCommandValidator>();
            //services.AddScoped<IValidator<VerifyOTPCommand>, VerifyOTPCommandValidator>();
            //services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
            //services.AddScoped<IValidator<ForgotPasswordCommand>, ForgotPasswordCommandValidator>();
            //services.AddScoped<IValidator<ForgotPasswordRecoveryTokenCommand>, ForgotPasswordRecoveryTokenCommandValidator>();
            //services.AddScoped<IValidator<RefreshTokenCommand>, RefreshTokenCommandValidator>();



            services.AddScoped<IValidator<CreateUniversityCommand>, CreateUniversityDtoValidator>();

            // AutoMapper
            //var mapperConfig = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<LogoutMappingProfile>();
            //    cfg.AddProfile<LogoutAllMappingProfile>();
            //    cfg.AddProfile<LogoutOthersMappingProfile>();
            //    cfg.AddProfile<SignUpMappingProfile>();
            //    cfg.AddProfile<SignInMappingProfile>();
            //    cfg.AddProfile<ResendSignInOTPProfile>();
            //    cfg.AddProfile<VerifyOTPMappingProfile>();
            //    cfg.AddProfile<ChangePasswordProfile>();
            //    cfg.AddProfile<ForgotPasswordProfile>();
            //    cfg.AddProfile<ForgotPasswordRecoveryTokenProfile>();
            //    cfg.AddProfile<RefreshTokenMappingProfile>();

            //    cfg.AddProfile<UniversityMappingProfile>();
            //    cfg.AddProfile<CommonMappingProfile>();
            //});

            //mapperConfig.AssertConfigurationIsValid();
            //var mapper = mapperConfig.CreateMapper();
            //services.AddSingleton<IMapper>(mapper);
            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Automatically add all profiles in the current assembly
                cfg.AddMaps(Assembly.GetExecutingAssembly());
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
    
        private static IServiceCollection AddValidatorServices(this IServiceCollection services, Type[] validatorTypes)
        {
            foreach (var validatorType in validatorTypes)
            {
                services.AddSingleton(typeof(IValidator<>), validatorType);
            }

            return services;
        }
    }
}