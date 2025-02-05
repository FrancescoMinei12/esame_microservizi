using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordini.Business.Abstractions;
using Ordini.Repository;
using Ordini.Shared.Configurations;

namespace Ordini.Business.Services;
public class OutboxProcessor : IOutboxProcessor
{
    private readonly OrdiniDbContext _ordiniDbContext;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly IProducer<string, string> _producer;
    public OutboxProcessor(OrdiniDbContext ordiniDbContext, IOptions<KafkaSettings> kafkaSettings, ILogger<OutboxProcessor> logger)
    {
        _ordiniDbContext = ordiniDbContext;
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            BatchSize = _kafkaSettings.BatchSize,
            MessageTimeoutMs = 5000,
            AllowAutoCreateTopics = _kafkaSettings.AllowAutoCreateTopic,
            Acks = Acks.All
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }
    public async Task ProcessOutboxAsync()
    {
        var messages = await _ordiniDbContext.TransactionalOutbox.Where(m => !m.Processed).Take(_kafkaSettings.BatchSize).ToListAsync();
        foreach (var message in messages)
        {
            try
            {
                await _producer.ProduceAsync("aggiornamento-totale-ordine", new Message<string, string> { Key = message.Id.ToString(), Value = message.Message });
                message.Processed = true;
                _logger.LogInformation($"Messaggio {message.Id} inviato al broker Kafka");
                await _ordiniDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante l'invio del messaggio {message.Id} al broker Kafka: {ex.Message}");
            }
        }
    }
}
