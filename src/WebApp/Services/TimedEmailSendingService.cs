using Alanyang.DotNetEmail.ApplicationCore.Interfaces;
using Alanyang.DotNetEmail.WebApp.Options;
using Microsoft.Extensions.Options;

namespace Alanyang.DotNetEmail.WebApp.Services;

public class TimedEmailSendingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TimedEmailSendingService> _logger;
    private readonly TimedEmailSendingOptions _options;
    private readonly TimeSpan _period;

    public TimedEmailSendingService(
        IServiceProvider serviceProvider,
        ILogger<TimedEmailSendingService> logger,
        IOptions<TimedEmailSendingOptions> options)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _options = options.Value;
        _period = TimeSpan.FromMinutes(_options.PeriodInMinute);
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

        await scopedService.SendEmailAsync(_options.SendsPerTime);
    }
}