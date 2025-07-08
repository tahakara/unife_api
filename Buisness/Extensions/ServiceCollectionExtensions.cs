using AutoMapper;
using Buisness.Abstract.ServicesBase;
using Buisness.Behaviors;
using Buisness.Features.CQRS.Universities.Commands.CreateUniversity;
using Buisness.Helpers;
using Buisness.Helpers.Base;
using Buisness.Mappings;
using Buisness.Mappings.Common;
using Buisness.Services;
using Buisness.Validators.FluentValidation.Validators.University.Request;
using Core.Utilities.BuisnessLogic.Base;
using DataAccess.Database;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Pipeline Behaviors - Ayrı olarak eklendi (sıralama önemli!)
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IValidator<CreateUniversityCommand>, CreateUniversityDtoValidator>();

            // Memory Cache
            services.AddMemoryCache();

            // Cache Service
            services.AddScoped<ICacheService, CacheService>();

            // AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UniversityMappingProfile>();
                cfg.AddProfile<CommonMappingProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
            
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            // Business Services
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IUniversityTypeService, UniversityTypeService>();
            services.AddScoped<IRegionService, RegionService>();

            // University Business Logic Helper
            services.AddScoped<IUniversityBuisnessLogicHelper, UniversityBusinessLogicHelper>();

            // Database Management Services (Test amaçlı)
            services.AddScoped<UnifeConnectionFactory>();

            return services;
        }
    }
}