using Buisness.Extensions;
using Buisness.Services.UtilityServices.Base.ObjectStorageServices;
using Buisness.Services.UtilityServices.ObjectStorageServices;
using Core.Database.Base;
using Core.ObjectStorage.Base;
using Core.Security.JWT.Extensions;
using DataAccess.Database;
using DataAccess.Database.Context;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPI.Compression;
using WebAPI.Compression.Zstd;
using WebAPI.HealthChecks;
using WebAPI.Mİddlewares;

// Serilog yapılandırması
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Unife API uygulaması başlatılıyor");

    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenLocalhost(5085); // Doğrudan localhost üzerinden dinle
    });
    // Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    //builder.Services.Configure<KestrelServerOptions>(options =>
    //{
    //    options.AllowSynchronousIO = true;
    //});
    // Controllers
    builder.Services.AddControllers(options =>
    {
        // Model validation configurations
    })
    .AddJsonOptions(options =>
    {
        // JSON serialization options
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;

        // Number handling - bu önemli kısım
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    });
    builder.Services.AddMemoryCache();
    builder.Services.AddEndpointsApiExplorer();

    #region Compression Services
    builder.Services.AddScoped<IZstdService, ZstdService>();
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.MimeTypes = new[]
        {
            "text/plain",
            "text/css",
            "application/javascript",
            "text/html",
            "application/xml",
            "text/xml",
            "application/json",
            "text/json",
        };

        options.Providers.Clear();
        options.Providers.Add<ZstdCompressionProvider>();
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.Providers.Add<WebAPI.Compression.DeflateCompressionProvider>();
    });

    builder.Services.Configure<ZstdCompressionProviderOptions>(options =>
    {
        options.Level = 10; // 1-22 
    });

    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.Fastest;
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.Fastest;
    });
    builder.Services.Configure<DeflateCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.Fastest; 
    });
    #endregion

    // Swagger Configuration
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Unife API",
            Version = "v1",
            Description = "Unife üniversite yönetim API'si",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "support@unife.com"
            }
        });

        // JWT Authentication support in Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

        // XML documentation
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://yourdomain.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
    });

    // Layer Services
    builder.Services.AddDataAccessServices(builder.Configuration);
    builder.Services.AddBusinessServices(builder.Configuration);

    // JWT Services
    builder.Services.AddJwtCore(builder.Configuration);
    builder.Services.AddScoped<ISessionJwtService, SessionJwtService>();

    // Authentication with JWT Bearer as the default scheme
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("database")
        .AddCheck("redis", () =>
        {
            try
            {
                using var scope = builder.Services.BuildServiceProvider().CreateScope();
                var objectStorageFactory = scope.ServiceProvider.GetRequiredService<IObjectStorageConnectionFactory>();
                var testResult = objectStorageFactory.TestConnectionAsync().GetAwaiter().GetResult();

                return testResult
                    ? HealthCheckResult.Healthy("Redis connection is healthy")
                    : HealthCheckResult.Unhealthy("Redis connection failed");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Redis connection error", ex);
            }
        });
    builder.Services.AddHttpContextAccessor();
    var app = builder.Build();

    // Database Migration
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<UnifeContext>();
            await context.Database.EnsureCreatedAsync();
            Log.Information("Veritabanı başarıyla başlatıldı");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Veritabanı başlatılırken hata oluştu");
            throw;
        }
    }

    // Object Storage (Redis) Initialization
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            Log.Information("Redis object storage bağlantıları test ediliyor");

            var factoryKeys = new[] { "cache", "session", "verification" };

            foreach (var key in factoryKeys)
            {
                try
                {
                    var objectStorageFactory = scope.ServiceProvider.GetRequiredKeyedService<IObjectStorageConnectionFactory>(key);

                    // Connection test
                    var connectionTestResult = await objectStorageFactory.TestConnectionAsync();
                    if (connectionTestResult)
                    {
                        Log.Information("Redis '{Key}' bağlantısı başarılı", key);

                        using var connection = await objectStorageFactory.CreateConnectionAsync();

                        var connectionInfo = new
                        {
                            IsConnected = connection.IsConnected,
                            ContainerName = connection.ContainerName,
                            CreatedAt = connection.CreatedAt,
                            Provider = "Redis",
                            Key = key
                        };

                        Log.Information("Redis '{Key}' bağlantı bilgileri: {@ConnectionInfo}", key, connectionInfo);

                        // Test basic operations
                        var testKey = $"startup_test_{key}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
                        var testValue = $"Redis {key} connection test successful";

                        try
                        {
                            await connection.SetStringAsync(testKey, testValue, TimeSpan.FromSeconds(30));
                            var retrievedValue = await connection.GetStringAsync(testKey);
                            await connection.DeleteAsync(testKey);

                            if (retrievedValue == testValue)
                            {
                                Log.Information("Redis '{Key}' operasyonel test başarılı", key);
                            }
                            else
                            {
                                Log.Warning("Redis '{Key}' operasyonel test başarısız - değer eşleşmiyor", key);
                            }
                        }
                        catch (Exception testEx)
                        {
                            Log.Warning(testEx, "Redis '{Key}' operasyonel test sırasında hata", key);
                        }

                        // Database size info
                        try
                        {
                            var dbSize = await connection.GetDatabaseSizeAsync();
                            Log.Information("Redis '{Key}' veritabanı boyutu: {Size} keys", key, dbSize);
                        }
                        catch (Exception sizeEx)
                        {
                            Log.Warning(sizeEx, "Redis '{Key}' veritabanı boyutu alınırken hata", key);
                        }
                    }
                    else
                    {
                        Log.Warning("Redis '{Key}' bağlantısı başarısız - uygulama devam ediyor", key);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Redis '{Key}' başlatılırken hata oluştu - uygulama devam ediyor", key);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Redis object storage başlatılırken hata oluştu - uygulama devam ediyor");
        }
    }

    // Middleware Pipeline
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Information;
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        };
    });


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unife API v1");
            c.RoutePrefix = "swagger";
        });
    }

    // Response Compression Middleware'i etkinleştirme
    app.UseResponseCompression();

    app.UseCors("AllowSpecificOrigins");
    app.UseHttpsRedirection();
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    //app.UseMiddleware<JwtAuthenticationMiddleware>();
    //app.UseMiddleware<PermissionAuthorizationMiddleware>();

    // Health Check endpoint
    app.MapHealthChecks("/health");

    app.MapControllers();

    Log.Information("Unife API başarıyla başlatıldı");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama beklenmedik şekilde sonlandı");
}
finally
{
    Log.CloseAndFlush();
}