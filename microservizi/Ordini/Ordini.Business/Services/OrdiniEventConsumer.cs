using Microsoft.Extensions.Logging;
using Ordini.Business.Abstractions;
using Ordini.Shared.Configurations;
using Confluent.Kafka;
using Ordini.Shared;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace Ordini.Business.Services;
public class OrdiniEventConsumer : IOrdiniEventConsumer
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IOrdineService _ordineService;
    private readonly ILogger<OrdiniEventConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    public OrdiniEventConsumer(IOrdineService ordineService, IOptions<KafkaSettings> kafkaSettings, ILogger<OrdiniEventConsumer> logger)
    {
        _ordineService = ordineService;
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = _kafkaSettings.EnableAutoCommit,
            StatisticsIntervalMs = _kafkaSettings.StatisticsIntervalMs,
            SessionTimeoutMs = _kafkaSettings.SessionTimeoutMs,
            MaxPollIntervalMs = _kafkaSettings.MaxPollIntervalMs,
            MetadataMaxAgeMs = _kafkaSettings.MetadataMaxAgeMs,
            EnablePartitionEof = _kafkaSettings.EnablePartitionEof
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }
    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_kafkaSettings.Topic);
        _logger.LogInformation($"Consumer avviato per il topic {_kafkaSettings.Topic}");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                if (consumeResult == null)
                {
                    _logger.LogWarning("Messaggio non valido");
                    continue;
                }
                _logger.LogInformation($"Messaggio ricevuto: {consumeResult.Message.Value}");

                var articoloEvent = JsonConvert.DeserializeObject<ArticoloAggiornatoEvent>(consumeResult.Message.Value);
                if (articoloEvent == null)
                {
                    _logger.LogWarning("Messaggio non valido");
                    continue;
                }
                _logger.LogInformation($"ArticoloId: {articoloEvent.ArticoloId}, NuovoPrezzo: {articoloEvent.NuovoPrezzo}");
                await _ordineService.RicalcolaTotaleOrdiniAsync(articoloEvent.ArticoloId, articoloEvent.NuovoPrezzo, cancellationToken);
                _consumer.Commit(consumeResult);
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Errore nel consumo del messaggio: {e.Error.Reason}");
                await Task.Delay(5000, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"Errore generico nel consumo del messaggio: {e}");
                await Task.Delay(5000, cancellationToken);
            }
        }
        _consumer.Close();
    }
}
