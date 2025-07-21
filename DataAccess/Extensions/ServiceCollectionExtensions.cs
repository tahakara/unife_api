using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core.Database.Base;
using Core.ObjectStorage.Base;
using Core.ObjectStorage.Base.Redis;
using Core.Database;
using Core.ObjectStorage;
using Core.Concrete.EntityFramework;
using Domain.Repositories.Abstract.Base;
using Domain.Repositories.Concrete.ObjectStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core.Database.Context;
using Core.ObjectStorage.Redis;
using Core.Abstract.Repositories.UniversityModuleRepositories;
using Core.Abstract.Repositories;
using Core.Concrete.EntityFramework.UniversityModuleDal;
using Core.Abstract.Repositories.AuthorizationModuleRepositories;
using Core.Concrete.EntityFramework.AuthorizationModuleDal;
using Core.Concrete.EntityFramework.AuthorizationModuleDal.SecurityEventDal;
using Core.Abstract.Repositories.AuthorizationModuleRepositories.SecurityEventRepositories;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Connection Factory
            services.AddScoped<IDbConnectionFactory<UnifeContext>, UnifeConnectionFactory>();
            
            // Redis Connection Factories for different storage types
            services.AddScoped<IObjectStorageConnectionFactory>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<ObjectStorageConnectionFactoryBase>>();
                return new GenericRedisConnectionFactory(configuration, logger, RedisStorageType.Cache);
            });

            // Named Redis Connection Factories
            services.AddKeyedScoped<IObjectStorageConnectionFactory>("cache", (provider, key) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<ObjectStorageConnectionFactoryBase>>();
                return new GenericRedisConnectionFactory(configuration, logger, RedisStorageType.Cache);
            });

            services.AddKeyedScoped<IObjectStorageConnectionFactory>("session", (provider, key) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var logger = provider.GetRequiredService<ILogger<ObjectStorageConnectionFactoryBase>>();
                return new GenericRedisConnectionFactory(configuration, logger, RedisStorageType.Session);
            });
            
            // Object Storage Generic Repository
            services.AddScoped(typeof(IObjectStorageRepository<>), typeof(ObjectStorageRepositoryBase<>));
            
            // DbContext registration
            services.AddDbContext<UnifeContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("UnifeDatabase") 
                    ?? throw new InvalidOperationException("Connection string 'UnifeDatabase' not found.");
                
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout(60);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                });

                // Development ortamında detaylı logging
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            // Database Repositories
            #region University Module Repositories
            
            #region Authorization Module Repositories
            services.AddScoped<IAdminRepository, EfAdminDal>();
            services.AddScoped<IStaffRepository, EfStaffDal>();
            services.AddScoped<IStudentRepository, EfStudentDal>();
            
            #region Security Event Repositories
            services.AddScoped<ISecurityEventRepository, EfSecurityEventDal>();
            services.AddScoped<ISecurityEventTypeRepository, EfSecurityEventTypeDal>();
            #endregion
            
            #endregion
            #endregion

            services.AddScoped<IUniversityRepository, EfUniversityDal>();
            services.AddScoped<IUniversityTypeRepository, EfUniversityTypeDal>();
            services.AddScoped<IRegionRepository, EfRegionDal>();
            //services.AddScoped<ICommunicationCategoryRepository, EfCommunicationCategoryDal>();
            //services.AddScoped<ICommunicationTypeRepository, EfCommunicationTypeDal>();
            //services.AddScoped<IAddressTypeRepository, EfAddressTypeDal>();
            //services.AddScoped<IAcademicDepartmentTypeRepository, EfAcademicDepartmentTypeDal>();
            //services.AddScoped<IAcademicianTitleRepository, EfAcademicianTitleDal>();
            //services.AddScoped<IAuditLogTypeRepository, EfAuditLogTypeDal>();
            //services.AddScoped<IUniversityCommunicationRepository, EfUniversityCommunicationDal>();
            //services.AddScoped<IUniversityAddressRepository, EfUniversityAddressDal>();
            //services.AddScoped<IFacultyRepository, EfFacultyDal>();
            //services.AddScoped<IAcademicDepartmentRepository, EfAcademicDepartmentDal>();
            //services.AddScoped<IUniversityFacultyDepartmentRepository, EfUniversityFacultyDepartmentDal>();
            //services.AddScoped<IAcademicianRepository, EfAcademicianDal>();
            //services.AddScoped<IRectorRepository, EfRectorDal>();
            
            return services;
        }
    }
}