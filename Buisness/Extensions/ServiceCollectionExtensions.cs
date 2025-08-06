using AutoMapper;
using Buisness.Behaviors;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Helpers.BuisnessLogicHelpers.Auth;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Buisness.Helpers.BuisnessLogicHelpers.UniversityBuisnessLogicHelper;
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
    /// <summary>
    /// Contains extension methods for registering Buisness layer services into the DI container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all Buisness layer services, validators, mappers, helpers, and utilities into the DI container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <param name="configuration">Optional application configuration.</param>
        /// <returns>The IServiceCollection for chaining.</returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            AddMediatRWithPipelineBehaviors(services);
            AddFluentValidation(services);
            AddAutoMapper(services);
            AddCoreBusinessServices(services, configuration);

            return services;
        }

        /// <summary>
        /// Registers MediatR and related pipeline behaviors.
        /// </summary>
        private static void AddMediatRWithPipelineBehaviors(IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
        }

        /// <summary>
        /// Registers FluentValidation validators.
        /// </summary>
        private static void AddFluentValidation(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Example explicit validator registration
            services.AddScoped<IValidator<CreateUniversityCommand>, CreateUniversityDtoValidator>();
        }

        /// <summary>
        /// Configures AutoMapper with profiles from the current assembly.
        /// </summary>
        private static void AddAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });

            mapperConfig.AssertConfigurationIsValid();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);
        }

        /// <summary>
        /// Registers core Buisness services, helpers, utilities, JWT core services and custom Redis connections.
        /// </summary>
        private static void AddCoreBusinessServices(IServiceCollection services, IConfiguration? configuration)
        {
            // Caching & JWT
            services.AddScoped<ICacheService, UnifeCacheService>();
            services.AddScoped<ISessionJwtService, SessionJwtService>();
            services.AddSingleton<IPasswordUtility, PasswordUtility>();
            services.AddSingleton<IOTPUtilitiy, OTPUtilitiy>();

            // Core Business Entity Services
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISecurityEventService, SecurityEventService>();
            services.AddScoped<ISecurityEventTypeService, SecurityEventTypeService>();

            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUniversityTypeService, UniversityTypeService>();
            services.AddScoped<IRegionService, RegionService>();

            // Business Logic Helpers
            services.AddScoped<IAuthBuisnessLogicHelper, AuthBuisnessLogicHelper>();
            services.AddScoped<IUniversityBuisnessLogicHelper, UniversityBusinessLogicHelper>();

            // DB Connections
            services.AddScoped<UnifeConnectionFactory>();

            // JWT Core Extension
            services.AddJwtCore(configuration);

            // Utility Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOTPCodeService, VerificationCodeService>();

            services.AddKeyedScoped<IObjectStorageConnectionFactory, GenericRedisConnectionFactory>("verification", (sp, key) =>
                new GenericRedisConnectionFactory(
                    sp.GetRequiredService<IConfiguration>(),
                    sp.GetRequiredService<ILogger<GenericRedisConnectionFactory>>(),
                    RedisStorageType.VerificationCode));
        }
    }
}
