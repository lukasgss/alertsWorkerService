using WorkerService.Interfaces;

namespace WorkerService;

public class Worker : BackgroundService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public Worker(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using IServiceScope scope = _serviceScopeFactory.CreateScope();

		IScopedProcessingService scopedProcessingService =
			scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

		scopedProcessingService.Execute(stoppingToken);
	}
}