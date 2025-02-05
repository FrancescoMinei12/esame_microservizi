using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Pagamenti.Business.Abstractions;
using Pagamenti.Shared.Configurations;
using System.Threading;

namespace Pagamenti.BackgroundServices;
public class PagamentiConsumerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PagamentiConsumerBackgroundService> _logger;
    private readonly KafkaSettings _kafkaSettings;
    public PagamentiConsumerBackgroundService(IServiceProvider serviceProvider, IOptions<KafkaSettings> kafkaSettings, ILogger<PagamentiConsumerBackgroundService> logger)
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
                using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = _kafkaSettings.BootstrapServers }).Build();
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
        _logger.LogInformation("Avvio del Consumer Kafka per aggiornamento pagamenti.");
        bool kafkaReady = false;
        while (!cancellationToken.IsCancellationRequested && !kafkaReady)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IPagamentiEventConsumer>();
                try
                {
                    _logger.LogInformation("Sto per avviare il consumer Kafka...");
                    await consumer.ConsumeAsync(cancellationToken);
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
