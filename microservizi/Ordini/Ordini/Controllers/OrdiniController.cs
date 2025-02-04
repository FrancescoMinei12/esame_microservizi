using Microsoft.AspNetCore.Mvc;
using Ordini.Business.Abstractions;
using Ordini.Shared;
using Inventario.ClientHttp.Abstraction;

namespace Ordini.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdiniController : Controller
{
    private readonly IBusiness _business;
    private readonly ILogger<OrdiniController> _logger;
    private readonly IInventarioClientHttp _inventarioClientHttp;

    public OrdiniController(IBusiness business, ILogger<OrdiniController> logger, IInventarioClientHttp inventarioClientHttp)
    {
        _business = business;
        _logger = logger;
        _inventarioClientHttp = inventarioClientHttp;
    }

    [HttpPost(Name = "CreateOrdine")]
    public async Task<ActionResult> CreateOrdine(int fk_cliente, decimal totale, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreateOrdineAsync(fk_cliente, totale, cancellationToken);
            return new JsonResult(new { message = "Ordine creato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpPost(Name = "AggiornaTotale")]
    public async Task<ActionResult> AggiornaTotale(int id, decimal nuovoTotale, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.AggiornaTotaleOrdineAsync(id, nuovoTotale, cancellationToken);
            return new JsonResult(new { message = "Totale ordine aggiornato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'aggiornamento del totale dell'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpPost(Name = "CreateOrdineCompleto")]
    public async Task<ActionResult> CreateOrdineCompleto(int idCliente, [FromBody] List<ProdottoQuantita> prodotti, int metodoPagamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreateOrdineCompletoAsync(idCliente, prodotti, metodoPagamentoId, cancellationToken);
            return new JsonResult(new { message = "Ordine completo creato con successo!" }) { StatusCode = 201 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'ordine completo.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetOrdineById")]
    public async Task<ActionResult<OrdineDto?>> GetOrdineById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var ordine = await _business.GetOrdineByIdAsync(id, cancellationToken);
            if (ordine == null)
                return new JsonResult(new { message = $"Ordine con ID '{id}' non trovato." }) { StatusCode = 404 };
            return new JsonResult(ordine) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetAllOrdini")]
    public async Task<ActionResult<List<OrdineDto>>> GetAllOrdini(CancellationToken cancellationToken = default)
    {
        try
        {
            var ordini = await _business.GetAllOrdiniAsync(cancellationToken);
            if (ordini == null || ordini.Count == 0)
                return new JsonResult(new { message = "Nessun ordine trovato." }) { StatusCode = 404 };
            return new JsonResult(ordini) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero degli ordini.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpPut(Name = "UpdateOrdine")]
    public async Task<ActionResult> UpdateOrdine(int id, [FromBody] OrdineDto ordineDto, CancellationToken cancellationToken = default)
    {
        if (ordineDto == null)
        {
            _logger.LogWarning("Richiesta non valida: il corpo dell'ordine è nullo.");
            return new JsonResult(new { message = "Dati ordine non validi." }) { StatusCode = 400 };
        }
        try
        {
            await _business.UpdateOrdineAsync(id, ordineDto.Totale, ordineDto.Fk_cliente, cancellationToken);
            _logger.LogInformation($"Ordine con ID '{id}' aggiornato con successo.");
            return new JsonResult(new { message = $"Ordine con ID '{id}' aggiornato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento dell'ordine con ID '{id}'.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpDelete(Name = "DeleteOrdine")]
    public async Task<ActionResult> DeleteOrdine(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeleteOrdineAsync(id, cancellationToken);
            return new JsonResult(new { message = $"Ordine con ID '{id}' eliminato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione dell'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }
}
