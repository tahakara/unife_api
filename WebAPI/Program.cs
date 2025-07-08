using Buisness.Extensions;
using Core.Database.Base;
using DataAccess.Context;
using DataAccess.Database;
using DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Reflection;
using WebAPI.HealthChecks;
using WebAPI.Middleware;
using System.Text.Json;
using System.Text.Json.Serialization;

// Serilog yapılandırması
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("UnifeAPI Enterprise uygulaması başlatılıyor");

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
    builder.Services.AddEndpointsApiExplorer();

    // Swagger - Gelişmiş konfigürasyon
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "UnifeAPI Enterprise",
            Version = "v1",
            Description = "Enterprise seviyede üniversite yönetim API'si",
            Contact = new OpenApiContact
            {
                Name = "API Support",
                Email = "support@unifeapi.com"
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
    builder.Services.AddBusinessServices();

    // Health Checks - Özel health check ile
    builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("database")
        .AddCheck("memory", () =>
        {
            var allocated = GC.GetTotalMemory(false);
            var data = new Dictionary<string, object>
            {
            { "allocated", allocated },
            { "gen0", GC.CollectionCount(0) },
            { "gen1", GC.CollectionCount(1) },
            { "gen2", GC.CollectionCount(2) }
            };

            return allocated < 1024 * 1024 * 100 * 1.024// 1gb
                ? HealthCheckResult.Healthy("Memory usage is normal", data)
                : HealthCheckResult.Degraded("Memory usage is high", data: data);
        });

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

    // Middleware Pipeline
    // app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "UnifeAPI v1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseCors("AllowSpecificOrigins");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    
    // Health Check endpoint
    app.MapHealthChecks("/health");
    
    app.MapControllers();

    Log.Information("UnifeAPI Enterprise başarıyla başlatıldı");
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
