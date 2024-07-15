using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ServerLibrary;

namespace Server.Services
{
    public class OtpCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public OtpCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_checkInterval, stoppingToken);
                await CleanupExpiredOtps(stoppingToken);
            }
        }

        private async Task CleanupExpiredOtps(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Glo2GoDbContext>();
                var expirationTime = DateTime.UtcNow.AddMinutes(-5);

                var expiredOtps = dbContext.OTPs.Where(o => o.CreatedAt < expirationTime);

                dbContext.OTPs.RemoveRange(expiredOtps);
                await dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
