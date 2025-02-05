using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pagamenti.Business.Abstractions;
using Pagamenti.Shared;
using Pagamenti.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagamenti.Business.Services;
public class PagamentiEventConsumer : IPagamentiEventConsumer
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly IPagamentiService _pagamentiService;
    private readonly ILogger<PagamentiEventConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    public PagamentiEventConsumer(IOptions<KafkaSettings> kafkaSettings, IPagamentiService pagamentiService, ILogger<PagamentiEventConsumer> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _pagamentiService = pagamentiService;
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
        _consumer.Subscribe("aggiornamento-totale-ordine");
        _logger.LogInformation($"Consumer avviato per il topic {_kafkaSettings.Topic}");
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5));
                if (consumeResult == null)
                {
                    _logger.LogWarning("Messaggio non valido");
                    continue;
                }
                if (consumeResult.Message?.Value == null)
                {
                    _logger.LogWarning("Messaggio non valido: Message.Value è null");
                    continue;
                }
                _logger.LogInformation($"Messaggio ricevuto: {consumeResult.Message.Value}");
                var pagamentoEvent = JsonConvert.DeserializeObject<PagamentoEvent>(consumeResult.Message.Value);
                if (pagamentoEvent == null)
                {
                    _logger.LogWarning("Messaggio non valido");
                    continue;
                }
                _logger.LogInformation($"Ricalcolo totale pagamenti per l'ordine {pagamentoEvent.OrdineId} con il nuovo prezzo {pagamentoEvent.NuovoTotale}");
                await _pagamentiService.RicalcolaTotalePagamentiAsync(pagamentoEvent.OrdineId, pagamentoEvent.NuovoTotale, cancellationToken);
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
