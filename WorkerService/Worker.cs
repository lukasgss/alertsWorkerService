using Application.Common.Interfaces;

namespace WorkerService;

public class Worker : BackgroundService
{
    private readonly IAlertsQueueService _alertsQueueService;
    public Worker(IAlertsQueueService alertsQueueService)
    {
        _alertsQueueService = alertsQueueService ?? throw new ArgumentNullException(nameof(alertsQueueService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _alertsQueueService.ConsumeAlerts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
