using Inventario.Business.Abstractions;

namespace Inventario.BackgroundServices;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxBackgroundService> _logger;
    public OutboxBackgroundService(IServiceProvider serviceProvider, ILogger<OutboxBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var outboxProcessor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();
                try
                {
                    await outboxProcessor.ProcessOutboxAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Errore nel processamento dell'outbox: {ex.Message}");
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
