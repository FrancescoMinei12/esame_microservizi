using Microsoft.AspNetCore.Mvc;
using Ordini.Business.Abstractions;
using Ordini.Shared;

namespace Ordini.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class OrdiniController : Controller
{
    private readonly IBusiness _business;
    private readonly ILogger<OrdiniController> _logger;

    public OrdiniController(IBusiness business, ILogger<OrdiniController> logger)
    {
        _business = business;
        _logger = logger;
    }

    [HttpPost(Name = "CreateOrdine")]
    public async Task<ActionResult> CreateOrdine(int fk_cliente, decimal totale, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreateOrdineAsync(fk_cliente, totale, cancellationToken);
            return Ok("Ordine creato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione dell'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetOrdineById")]
    public async Task<ActionResult<OrdineDto?>> GetOrdineById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var ordine = await _business.GetOrdineByIdAsync(id, cancellationToken);
            if (ordine == null)
                return NotFound($"Ordine con ID '{id}' non trovato.");

            return Ok(ordine);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dell'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllOrdini")]
    public async Task<ActionResult<List<OrdineDto>>> GetAllOrdini(CancellationToken cancellationToken = default)
    {
        try
        {
            var ordini = await _business.GetAllOrdiniAsync(cancellationToken);
            if (ordini == null || ordini.Count == 0)
                return NotFound("Nessun ordine trovato.");

            return Ok(ordini);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero degli ordini.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpPut(Name = "UpdateOrdine")]
    public async Task<ActionResult> UpdateOrdine(int id, [FromBody] OrdineDto ordineDto, CancellationToken cancellationToken = default)
    {
        if (ordineDto == null)
        {
            _logger.LogWarning("Richiesta non valida: il corpo dell'ordine è nullo.");
            return BadRequest("Dati ordine non validi.");
        }

        try
        {
            await _business.UpdateOrdineAsync(id, ordineDto.Totale, ordineDto.Fk_cliente, cancellationToken);
            _logger.LogInformation($"Ordine con ID '{id}' aggiornato con successo.");
            return Ok($"Ordine con ID '{id}' aggiornato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento dell'ordine con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpDelete(Name = "DeleteOrdine")]
    public async Task<ActionResult> DeleteOrdine(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeleteOrdineAsync(id, cancellationToken);
            return Ok($"Ordine con ID '{id}' eliminato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione dell'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
