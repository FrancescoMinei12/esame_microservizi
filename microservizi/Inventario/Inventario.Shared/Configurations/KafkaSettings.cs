namespace Inventario.Shared.Configurations;
public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public int Retries { get; set; }
    public int BatchSize { get; set; }
    public int PollIntervalSeconds { get; set; }
    public bool AllowAutoCreateTopic { get; set; } = false; 

}
