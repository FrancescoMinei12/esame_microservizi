using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ordini.Business.Abstractions;
using Ordini.Shared.Configurations;

namespace Ordini.BackgroundServices;

public class OrdiniConsumerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrdiniConsumerBackgroundService> _logger;
    private readonly KafkaSettings _kafkaSettings; 

    public OrdiniConsumerBackgroundService(IServiceProvider serviceProvider, IOptions<KafkaSettings> kafkaSettings, ILogger<OrdiniConsumerBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value; 
    }
    private async Task WaitForKafkaAsync(CancellationToken cancellationToken)
    {
        int maxRetries = 10;
        int delayMs = 5000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers =  _kafkaSettings.BootstrapServers}).Build();
                _logger.LogInformation("Kafka è pronto!");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Tentativo {attempt}/{maxRetries} - Kafka non ancora pronto: {ex.Message}");
                await Task.Delay(delayMs, cancellationToken);
            }
        }
        _logger.LogError("Kafka non è stato raggiunto dopo numerosi tentativi. Controlla la connessione.");
        throw new Exception("Impossibile connettersi a Kafka.");
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await WaitForKafkaAsync(cancellationToken);
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