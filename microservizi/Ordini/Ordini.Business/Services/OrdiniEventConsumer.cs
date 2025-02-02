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
    private readonly IOrdineService _ordineService;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<OrdiniEventConsumer> _logger;
    public OrdiniEventConsumer(IOrdineService ordineService, IOptions<KafkaSettings> kafkaSettings, ILogger<OrdiniEventConsumer> logger)
    {
        _ordineService = ordineService;
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
    }
    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_kafkaSettings.Topic);
        _logger.LogInformation($"Consumer avviato per il topic {_kafkaSettings.Topic}");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);
                _logger.LogInformation($"Messaggio ricevuto: {consumeResult.Message.Value}");

                var articoloEvent = JsonConvert.DeserializeObject<ArticoloAggiornatoEvent>(consumeResult.Message.Value);
                _logger.LogInformation($"Messaggio raw ricevuto: {consumeResult.Message.Value}");
                if (articoloEvent == null)
                {
                    _logger.LogWarning("Messaggio non valido");
                    continue;
                }
                _logger.LogInformation($"ArticoloId: {articoloEvent.ArticoloId}, NuovoPrezzo: {articoloEvent.NuovoPrezzo}");
                await _ordineService.RicalcolaTotaleOrdiniAsync(articoloEvent.ArticoloId, articoloEvent.NuovoPrezzo, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError($"Errore nel consumo del messaggio: {e.Message}");
            }
        }

        consumer.Close();
    }
}
