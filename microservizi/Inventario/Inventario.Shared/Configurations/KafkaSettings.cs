namespace Inventario.Shared.Configurations;
public class KafkaSettings
{
    public string BootstrapServers { get; set; }
    public string Topic { get; set; }
    public int Retries { get; set; }
    public int BatchSize { get; set; }
    public int PollIntervalSeconds { get; set; }
}
