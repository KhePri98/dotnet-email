using Alanyang.DotNetEmail.ApplicationCore.Interfaces;

namespace Alanyang.DotNetEmail.WebApp.Services;

public class TimedEmailSendingService : BackgroundService
{
    private readonly IEmailSendingQueue _emailSendingQueue;
    private readonly ILogger<TimedEmailSendingService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromSeconds(30);

    public TimedEmailSendingService(
        IEmailSendingQueue emailSendingQueue,
        ILogger<TimedEmailSendingService> logger)
    {
        _emailSendingQueue = emailSendingQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        while (
            !stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            await _emailSendingQueue.SendEmailAsync();

            _logger.LogInformation("Consume Scoped Service Hosted Service running.");
        }
    }
}