using Microsoft.Extensions.Options;
using Ordini.Business.Abstractions;
using Ordini.Shared.Configurations;
using System.Threading;

namespace Ordini.BackgroundServices;
public class OrdiniProducerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrdiniConsumerBackgroundService> _logger;
    public OrdiniProducerBackgroundService(IServiceProvider serviceProvider, ILogger<OrdiniConsumerBackgroundService> logger)
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
