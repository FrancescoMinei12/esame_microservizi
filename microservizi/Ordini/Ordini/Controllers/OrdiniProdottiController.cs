using Ordini.Business.Abstractions;
using Ordini.Shared;
using Microsoft.AspNetCore.Mvc;
using Inventario.ClientHttp.Abstraction;
using Inventario.Shared;

namespace Ordini.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdiniProdottiController : Controller
{
    private readonly IBusiness _business;
    private readonly ILogger<OrdiniProdottiController> _logger;
    public OrdiniProdottiController(IBusiness business, ILogger<OrdiniProdottiController> logger)
    {
        _business = business;
        _logger = logger;
    }

    [HttpPost(Name = "AddOrdineProdotto")]
    public async Task<ActionResult> AddOrdineProdotto(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.AddOrdineProdottoAsync(fk_ordine, fk_prodotto, quantita, cancellationToken);
            return new JsonResult(new { message = "Prodotto aggiunto all'ordine con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiunta del prodotto all'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetOrdineProdottoById")]
    public async Task<ActionResult<OrdineProdottiDto?>> GetOrdineProdottoById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var ordineProdotto = await _business.GetOrdineProdottoByIdAsync(id, cancellationToken);
            if (ordineProdotto == null)
                return new JsonResult(new { message = $"OrdineProdotto con ID '{id}' non trovato." }) { StatusCode = 404 };
            return new JsonResult(ordineProdotto) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'OrdineProdotto.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetProdottiByOrdine")]
    public async Task<ActionResult<List<OrdineProdottiDto>>> GetProdottiByOrdine(int fk_ordine, CancellationToken cancellationToken = default)
    {
        try
        {
            var prodotti = await _business.GetProdottiByOrdineAsync(fk_ordine, cancellationToken);
            if (prodotti == null || prodotti.Count == 0)
                return new JsonResult(new { message = $"Nessun prodotto trovato per l'ordine con ID '{fk_ordine}'." }) { StatusCode = 404 };
            return new JsonResult(prodotti) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei prodotti per l'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetProdottiDetailsByOrdine")]
    public async Task<ActionResult<List<ArticoloDto>>> GetProdottiDetailsByOrdine(int fk_ordine, CancellationToken cancellationToken = default)
    {
        try
        {
            return new JsonResult(await _business.GetProdottiDetailsByOrdine(fk_ordine, cancellationToken)) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante il recupero dei dettagli dei prodotti per l'ordine con ID '{fk_ordine}'.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }


    [HttpPut(Name = "UpdateOrdineProdotto")]
    public async Task<ActionResult> UpdateOrdineProdotto(int id, [FromBody] OrdineProdottiDto ordineProdottoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.UpdateOrdineProdottoAsync(id, ordineProdottoDto.Quantita, ordineProdottoDto.Fk_ordine, ordineProdottoDto.Fk_prodotto, cancellationToken);
            _logger.LogInformation($"OrdineProdotto con ID '{id}' aggiornato con successo.");
            return new JsonResult(new { message = $"OrdineProdotto con ID '{id}' aggiornato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento di OrdineProdotto con ID '{id}'.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }


    [HttpDelete(Name = "RemoveProdottoFromOrdine")]
    public async Task<ActionResult> RemoveProdottoFromOrdine(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.RemoveProdottoFromOrdineAsync(id, cancellationToken);
            return new JsonResult(new { message = $"Prodotto con ID '{id}' rimosso dall'ordine con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la rimozione del prodotto dall'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }
}
