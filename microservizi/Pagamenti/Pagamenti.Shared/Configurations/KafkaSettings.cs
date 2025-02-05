namespace Pagamenti.Shared.Configurations;
public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
    public int Retries { get; set; }
    public int BatchSize { get; set; }
    public int PollIntervalSeconds { get; set; }
    public bool EnableAutoCommit { get; set; }
    public int StatisticsIntervalMs { get; set; }
    public int SessionTimeoutMs { get; set; }
    public int MaxPollIntervalMs { get; set; }
    public int MetadataMaxAgeMs { get; set; }
    public bool EnablePartitionEof { get; set; }
}
