using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Behaviors;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Helpers;
using Buisness.Helpers.Auth;
using Buisness.Helpers.Base;
using Buisness.Mappings;
using Buisness.Mappings.Common;
using Buisness.Services.EntityRepositoryServices;
using Buisness.Services.UtilityServices;
using Buisness.Services.UtilityServices.Abtract;
using Buisness.Validators.FluentValidation.Validators.University.Request;
using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Core.Security.JWT.Extensions;
using Core.Utilities.BuisnessLogic.Base;
using DataAccess.Database;
using DataAccess.ObjectStorage.Redis;
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
            services.AddScoped<IValidator<CreateUniversityCommand>, CreateUniversityDtoValidator>();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UniversityMappingProfile>();
                cfg.AddProfile<CommonMappingProfile>();
            });
            mapperConfig.AssertConfigurationIsValid();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            // *** UNIFE SERVICES - Sadeleştirilmiş Yapı ***
            services.AddScoped<ICacheService, UnifeCacheService>();
            services.AddScoped<ISessionJwtService, SessionJwtService>();

            // Business Services
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUniversityTypeService, UniversityTypeService>();
            services.AddScoped<IRegionService, RegionService>();

            // Business Logic Helper
            services.AddScoped<IAuthBuissnessLogicHelper, AuthBuissnessLogicHelper>();
            services.AddScoped<IUniversityBuisnessLogicHelper, UniversityBusinessLogicHelper>();

            // Database Management Services
            services.AddScoped<UnifeConnectionFactory>();

            // JWT Core services
            services.AddJwtCore(configuration);

            services.AddScoped<IEmailService, EmailService>();

            // Verification Code Service
            services.AddScoped<IVerificationCodeService, VerificationCodeService>();
            services.AddKeyedScoped<IObjectStorageConnectionFactory, GenericRedisConnectionFactory>("verificationcode", (sp, key) =>
    new GenericRedisConnectionFactory(
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ILogger<GenericRedisConnectionFactory>>(),
        RedisStorageType.VerificationCode));

            return services;
        }
    }
}