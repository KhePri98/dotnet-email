using Alanyang.DotNetEmail.ApplicationCore.Interfaces;

namespace Alanyang.DotNetEmail.WebApp.Services;

public class TimedEmailSendingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TimedEmailSendingService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(30);

    public TimedEmailSendingService(
        IServiceProvider serviceProvider,
        ILogger<TimedEmailSendingService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWorkAsync();

            _logger.LogInformation("TimedEmailSendingService working!");
        }
    }

    private async Task DoWorkAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IEmailSendingQueue>();

        await scopedService.SendEmailAsync();
    }
}