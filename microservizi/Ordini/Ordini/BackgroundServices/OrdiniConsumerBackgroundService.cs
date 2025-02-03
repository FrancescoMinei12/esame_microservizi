using Ordini.Business.Abstractions;

namespace Ordini.BackgroundServices;

public class OrdiniConsumerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrdiniConsumerBackgroundService> _logger;
    public OrdiniConsumerBackgroundService(IServiceProvider serviceProvider, ILogger<OrdiniConsumerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Avvio del Consumer Kafka per aggiornamento ordini.");
        bool kafkaReady = false;
        while (!cancellationToken.IsCancellationRequested && !kafkaReady)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IOrdiniEventConsumer>();
                try
                {
                    _logger.LogInformation("Sto per avviare il consumer Kafka...");
                    await consumer.ConsumeAsync(cancellationToken);  // Attende asincrono
                    kafkaReady = true;
                    _logger.LogInformation("Connessione Kafka stabilita correttamente e consumer attivo.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Errore nel background service: {ex.Message}");
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}