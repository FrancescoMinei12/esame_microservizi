using Inventario.Repository.Model;
using Inventario.Repository.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.Kafka.Abstractions.Clients;
using Utility.Kafka.Services;
using System.Text.Json;

namespace Inventario.Business.Kafka;

public class ProducerService : ProducerService<KafkaTopicsOutput>
{
    public ProducerService(ILogger<ProducerService> logger, IProducerClient producerClient, IAdministatorClient adminClient, IOptions<KafkaTopicsOutput> optionsTopics, IOptions<KafkaProducerServiceOptions> optionsProducerService, IServiceScopeFactory serviceScopeFactory) : base(logger, producerClient, adminClient, optionsTopics, optionsProducerService, serviceScopeFactory) { }

    protected override async Task OperationsAsync(CancellationToken cancellationToken)
    {
        Logger.LogInformation("Eseguendo OperationsAsync in InventarioProducerService...");
        await Task.CompletedTask;
    }
    public async Task PublishArticoloAggiornatoAsync(int id, string nome, int quantita, decimal prezzo, CancellationToken cancellationToken)
    {
        var evento = new
        {
            Event = "InventarioAggiornato",
            ArticoloId = id,
            Nome = nome,
            Quantita = quantita,
            Prezzo = prezzo,
            Timestamp = DateTime.UtcNow
        };

        string message = JsonSerializer.Serialize(evento);
        await ProducerClient.ProduceAsync(KafkaTopics.RifornimentoArticoliTopic, message, cancellationToken);
    }
}
