using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VehicleKhatabook.Services.Interfaces;

namespace VehicleKhatabook.Services.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceProvider serviceProvider, ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Calculate the next run time
                    var now = DateTime.Now;
                    var nextRunTime = now.Date.AddDays(1); // Next run is 2 minutes from midnight or the current time.
                    var timeUntilNextRun = nextRunTime - now;

                    if (timeUntilNextRun <= TimeSpan.Zero)
                    {
                        // If already past the scheduled time, calculate the next day's run
                        nextRunTime = now.AddDays(1);
                        timeUntilNextRun = nextRunTime - now;
                    }

                    // Log the next run time
                    _logger.LogInformation($"Notification Background Service will run at {nextRunTime:dd-MM-yyyy HH:mm:ss}.");

                    // Safeguard to avoid `Task.Delay` errors
                    if (timeUntilNextRun.TotalMilliseconds > 0)
                    {
                        await Task.Delay(timeUntilNextRun, stoppingToken);
                    }

                    // Execute the notification logic
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notificationService.CheckForExpirationsAndNotifyAsync();
                    }
                }
                catch (TaskCanceledException)
                {
                    // Handle graceful shutdown
                    _logger.LogInformation("Notification Background Service has been stopped.");
                    break;
                }
                catch (Exception ex)
                {
                    // Log unexpected errors
                    _logger.LogError(ex, "Error occurred while running expiration check.");
                }
            }
        }


    }
}
