using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Database.Context;

namespace WebAPI.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly UnifeContext _context;

        public DatabaseHealthCheck(UnifeContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Basit bir sorgu ile veritabanı bağlantısını test et
                await _context.Database.ExecuteSqlRawAsync("SELECT 47", cancellationToken);
                
                return HealthCheckResult.Healthy("Veritabanı bağlantısı başarılı");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Veritabanı bağlantısı başarısız", ex);
            }
        }
    }
}