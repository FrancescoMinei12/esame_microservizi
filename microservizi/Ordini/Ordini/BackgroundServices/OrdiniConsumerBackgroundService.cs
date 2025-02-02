using Ordini.Business.Abstractions;

namespace Ordini.BackgroundServices;

public class OrdiniConsumerBackgroundService : BackgroundService
{
    private readonly ILogger<OrdiniConsumerBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public OrdiniConsumerBackgroundService(ILogger<OrdiniConsumerBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Avvio del Consumer Kafka per aggiornamento ordini.");
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IOrdiniEventConsumer>();
                await consumer.ConsumeAsync(cancellationToken);
            }
            await Task.Delay(5000, cancellationToken);
        }
    }
}