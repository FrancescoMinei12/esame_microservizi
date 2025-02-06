using Inventario.ClientHttp.Abstraction;
using Microsoft.Extensions.Logging;
using Ordini.Business.Abstractions;
using Ordini.Repository.Abstraction;
using System.Text.Json;

namespace Ordini.Business.Services;
public class OrdineService : IOrdineService
{
    private readonly IRepository _repository;
    private readonly ILogger<OrdineService> _logger;
    private readonly IInventarioClientHttp _inventarioClientHttp;
    public OrdineService(IRepository repository, ILogger<OrdineService> logger, IInventarioClientHttp inventarioClientHttp)
    {
        _repository = repository;
        _logger = logger;
        _inventarioClientHttp = inventarioClientHttp;
    }
    public async Task RicalcolaTotaleOrdiniAsync(int articoloId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Ricalcolando totale per ArticoloId: {articoloId}");

        var ordini = await _repository.GetOrdiniByArticoloIdAsync(articoloId);
        _logger.LogInformation($"Trovati {ordini.Count} ordini per ArticoloId: {articoloId}");
        foreach (var ordine in ordini)
        {
            decimal nuovoTotale = await CalcolaTotale(ordine.Id, cancellationToken);
            _logger.LogInformation($"Nuovo totale per ordine {ordine.Id}: {nuovoTotale}");
            await _repository.AggiornaTotaleOrdineAsync(ordine.Id, nuovoTotale, cancellationToken);
            await _repository.AddTransactionalOutboxAsync(JsonSerializer.Serialize(new
            {
                OrdineId = ordine.Id,
                NuovoTotale = nuovoTotale
            }), cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
        }
        _logger.LogInformation("Totale ordini aggiornato con successo.");
    }
    private async Task<decimal> CalcolaTotale(int ordineId, CancellationToken cancellationToken)
    {
        var ordine = await _repository.ReadOrdineAsync(ordineId, cancellationToken);
        _logger.LogInformation($"Calcolando totale per ordine {ordineId}");
        if (ordine == null) return 0;
        var articoliNellOrdine = await _repository.GetProdottiByOrdineAsync(ordineId);
        if (articoliNellOrdine == null || articoliNellOrdine.Count == 0) return 0;
        decimal totale = 0;
        foreach (var articoloNellOrdine in articoliNellOrdine)
        {
            var articolo = await _inventarioClientHttp.GetArticoloAsync(articoloNellOrdine.Fk_prodotto, cancellationToken);
            if (articolo != null)
            {
                _logger.LogInformation($"ArticoloId: {articolo.Id}, Prezzo: {articolo.Prezzo}, Quantita nell'ordine: {articoloNellOrdine.Quantita}");
                totale += articoloNellOrdine.Quantita * articolo.Prezzo;
                _logger.LogInformation($"Totale parziale: {totale}");
            }
        }
        return totale;
    }
}
