namespace WorkerService.Interfaces;

public interface IScopedProcessingService
{
	void Execute(CancellationToken stoppingToken);
}