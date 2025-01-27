using Ordini.Business.Abstractions;
using Ordini.Repository.Model;
using Ordini.Shared;
using Microsoft.AspNetCore.Mvc;
using Ordini.ClientHttp.Abstraction;
using Inventario.ClientHttp.Abstraction;
using Inventario.Shared;

namespace Ordini.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdiniProdottiController : Controller
{
    private readonly IBusiness _business;
    private readonly ILogger<OrdiniProdottiController> _logger;
    private readonly IClientHttp _inventarioClientHttp;

    public OrdiniProdottiController(IBusiness business, ILogger<OrdiniProdottiController> logger, IClientHttp inventarioClientHttp)
    {
        _business = business;
        _logger = logger;
        _inventarioClientHttp = inventarioClientHttp;
    }

    [HttpPost(Name = "AddOrdineProdotto")]
    public async Task<ActionResult> AddOrdineProdotto(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.AddOrdineProdottoAsync(fk_ordine, fk_prodotto, quantita, cancellationToken);
            return Ok("Prodotto aggiunto all'ordine con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta del prodotto all'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetOrdineProdottoById")]
    public async Task<ActionResult<OrdineProdottiDto?>> GetOrdineProdottoById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var ordineProdotto = await _business.GetOrdineProdottoByIdAsync(id, cancellationToken);
            if (ordineProdotto == null)
                return NotFound($"OrdineProdotto con ID '{id}' non trovato.");

            return Ok(ordineProdotto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'OrdineProdotto.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetProdottiByOrdine")]
    public async Task<ActionResult<List<OrdineProdottiDto>>> GetProdottiByOrdine(int fk_ordine, CancellationToken cancellationToken = default)
    {
        try
        {
            var prodotti = await _business.GetProdottiByOrdineAsync(fk_ordine, cancellationToken);
            if (prodotti == null || prodotti.Count == 0)
                return NotFound($"Nessun prodotto trovato per l'ordine con ID '{fk_ordine}'.");

            return Ok(prodotti);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei prodotti per l'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetProdottiDetailsByOrdine")]
    public async Task<ActionResult<List<ArticoloDto>>> GetProdottiDetailsByOrdine(int fk_ordine, CancellationToken cancellationToken = default)
    {
        try
        {
            // Recupera i prodotti associati all'ordine
            var prodotti = await _business.GetProdottiByOrdineAsync(fk_ordine, cancellationToken);

            if (prodotti == null || prodotti.Count == 0)
            {
                _logger.LogWarning($"Nessun prodotto trovato per l'ordine con ID '{fk_ordine}'.");
                return NotFound($"Nessun prodotto trovato per l'ordine con ID '{fk_ordine}'.");
            }
            var tasks = prodotti.Select(async prodotto =>
            {
                var dettaglioProdotto = await _inventarioClientHttp.GetArticoloAsync(prodotto.Fk_prodotto, cancellationToken);
                if (dettaglioProdotto == null)
                {
                    _logger.LogWarning($"Dettagli non trovati per il prodotto con ID '{prodotto.Fk_prodotto}'.");
                    return null;
                }
                dettaglioProdotto.QuantitaDisponibile = prodotto.Quantita;
                return dettaglioProdotto;
            });
            var dettagliProdotti = (await Task.WhenAll(tasks))
                .Where(x => x != null) 
                .ToList();

            return Ok(dettagliProdotti);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante il recupero dei dettagli dei prodotti per l'ordine con ID '{fk_ordine}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }


    [HttpPut(Name = "UpdateOrdineProdotto")]
    public async Task<ActionResult> UpdateOrdineProdotto(int id, [FromBody] OrdineProdottiDto ordineProdottoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.UpdateOrdineProdottoAsync(id, ordineProdottoDto.Quantita, ordineProdottoDto.Fk_ordine, ordineProdottoDto.Fk_prodotto, cancellationToken);
            _logger.LogInformation($"OrdineProdotto con ID '{id}' aggiornato con successo.");
            return Ok($"OrdineProdotto con ID '{id}' aggiornato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento di OrdineProdotto con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }


    [HttpDelete(Name = "RemoveProdottoFromOrdine")]
    public async Task<ActionResult> RemoveProdottoFromOrdine(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.RemoveProdottoFromOrdineAsync(id, cancellationToken);
            return Ok($"Prodotto con ID '{id}' rimosso dall'ordine con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la rimozione del prodotto dall'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
