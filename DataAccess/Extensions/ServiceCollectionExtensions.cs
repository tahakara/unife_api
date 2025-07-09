using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Core.Database.Base;
using Core.ObjectStorage.Base;
using DataAccess.Context;
using DataAccess.Database;
using DataAccess.ObjectStorage;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Domain.Repositories.Abstract.Base;
using Domain.Repositories.Concrete.ObjectStorage;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Connection Factory
            services.AddScoped<IDbConnectionFactory<UnifeContext>, UnifeConnectionFactory>();
            
            // Object Storage Connection Factory
            services.AddScoped<IObjectStorageConnectionFactory, UnifeObjectStorageConnectionFactory>();
            
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