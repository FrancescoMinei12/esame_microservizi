using Confluent.Kafka;
using Inventario.Business.Abstractions;
using Inventario.Repository;
using Inventario.Shared.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Inventario.Business.Services;

public class OutboxProcessor : IOutboxProcessor
{
    private readonly InventarioDbContext _inventarioDbContext;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly IProducer<string, string> _producer;
    public OutboxProcessor(InventarioDbContext inventarioDbContext, IOptions<KafkaSettings> kafkaSettings, ILogger<OutboxProcessor> logger)
    {
        _inventarioDbContext = inventarioDbContext;
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
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
        var messages = await _inventarioDbContext.Outboxes.Where(m => !m.Processed).Take(_kafkaSettings.BatchSize).ToListAsync();
        foreach (var message in messages)
        {
            try
            {
                await _producer.ProduceAsync(_kafkaSettings.Topic, new Message<string, string> { Key = message.Id.ToString(), Value = message.Message });
                message.Processed = true;
                await _inventarioDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante l'invio del messaggio {message.Id} al broker Kafka: {ex.Message}");
            }
        }
    }
}
