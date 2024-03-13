using Application.Common.Interfaces.Services;
using WorkerService.Interfaces;

namespace WorkerService;

public class ScopedProcessingService : IScopedProcessingService
{
	private readonly IAlertsQueueService _alertsQueueService;

	public ScopedProcessingService(IAlertsQueueService alertsQueueService)
	{
		_alertsQueueService = alertsQueueService ?? throw new ArgumentNullException(nameof(alertsQueueService));
	}

	public void Execute(CancellationToken stoppingToken)
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