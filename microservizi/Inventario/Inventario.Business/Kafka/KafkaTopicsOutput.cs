using Microsoft.Extensions.DependencyInjection;

namespace Inventario.Business.Kafka;

public class KafkaTopicsOutput : AbstractKafkaTopics
{
    public string RifornimentoArticoliTopic { get; set; } = "rifornimento-articoli";
    public override IEnumerable<string> GetTopics() => new List<string> { RifornimentoArticoliTopic };
}
