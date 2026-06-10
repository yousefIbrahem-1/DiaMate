using DiaMate.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class UserCleanupBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public UserCleanupBackgroundService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var cleanupService =
                scope.ServiceProvider
                     .GetRequiredService<IUserCleanupService>();

            await cleanupService
                .DeleteExpiredUnconfirmedUsersAsync();

            await Task.Delay(
                TimeSpan.FromDays(1),
                stoppingToken);
        }
    }
}