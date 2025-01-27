using Microsoft.AspNetCore.Mvc;
using Pagamenti.Business.Abstractions;
using Pagamenti.Shared;
using Ordini.ClientHttp;
using Microsoft.Extensions.Logging;
using Ordini.ClientHttp.Abstraction;

namespace Pagamenti.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class PagamentiController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<PagamentiController> _logger;
    private readonly IOrdiniClientHttp _ordiniClientHttp;

    public PagamentiController(IBusiness business, ILogger<PagamentiController> logger, IOrdiniClientHttp ordiniClientHttp)
    {
        _business = business;
        _logger = logger;
        _ordiniClientHttp = ordiniClientHttp;
    }

    [HttpPost(Name = "CreatePagamento")]
    public async Task<ActionResult> CreatePagamento(decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreatePagamentoAsync(importo, dataPagamento, fkOrdine, fkMetodoPagamento, cancellationToken);
            return Ok("Pagamento creato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetPagamentoById")]
    public async Task<ActionResult<PagamentoDto?>> GetPagamentoById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamento = await _business.GetPagamentoByIdAsync(id, cancellationToken);
            if (pagamento == null)
                return NotFound($"Pagamento con ID '{id}' non trovato.");

            return Ok(pagamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllPagamenti")]
    public async Task<ActionResult<List<PagamentoDto>>> GetAllPagamenti(CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamenti = await _business.GetAllPagamentiAsync(cancellationToken);
            if (pagamenti == null || pagamenti.Count == 0)
                return NotFound("Nessun pagamento trovato.");

            return Ok(pagamenti);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei pagamenti.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllOrdiniClientHttp")]
    public async Task<IActionResult> GetAllOrdiniClientHttp(CancellationToken cancellationToken = default)
    {
        try
        {
            var ordini = await _ordiniClientHttp.GetAllOrdiniAsync(cancellationToken);
            if (ordini == null || !ordini.Any())
            {
                return NotFound("Nessun ordine trovato.");
            }
            return Ok(ordini);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Errore durante il recupero degli ordini: {ex.Message}");
        }
    }

    [HttpGet(Name = "GetOrdineByPagamento")]
    public async Task<IActionResult> GetOrdineByPagamento(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamento = await _business.GetPagamentoByIdAsync(id, cancellationToken);
            if (pagamento == null)
                return NotFound($"Pagamento con ID '{id}' non trovato.");
            var ordine = await _ordiniClientHttp.GetOrdineByIdAsync(pagamento.Fk_Ordine, cancellationToken);
            if (ordine == null)
                return NotFound($"Ordine con ID '{pagamento.Fk_Ordine}' non trovato.");
            return Ok(new { Pagamento = pagamento, Ordine = ordine });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei dettagli del pagamento e dell'ordine.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpPut(Name = "UpdatePagamento")]
    public async Task<ActionResult> UpdatePagamento(int id, [FromBody] PagamentoDto pagamentoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.UpdatePagamentoAsync(
                id,
                pagamentoDto.Importo,
                pagamentoDto.DataPagamento,
                pagamentoDto.Fk_Ordine,
                pagamentoDto.Fk_MetodoPagamento,
                cancellationToken
            );
            _logger.LogInformation($"Pagamento con ID '{id}' aggiornato correttamente.");
            return Ok($"Pagamento con ID '{id}' aggiornato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del pagamento con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }


    [HttpDelete(Name = "DeletePagamento")]
    public async Task<ActionResult> DeletePagamento(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeletePagamentoAsync(id, cancellationToken);
            return Ok($"Pagamento con ID '{id}' eliminato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
