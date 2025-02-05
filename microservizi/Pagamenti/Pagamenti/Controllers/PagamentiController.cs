using Microsoft.AspNetCore.Mvc;
using Pagamenti.Business.Abstractions;
using Pagamenti.Shared;
using Ordini.ClientHttp;
using Microsoft.Extensions.Logging;
using Ordini.ClientHttp.Abstraction;
using Pagamenti.Repository.Model;

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
    public async Task<ActionResult> CreatePagamento([FromBody] PagamentoDto pagamentoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (pagamentoDto == null)
            {
                return new JsonResult(new { error = "I dati del pagamento non possono essere null." }) { StatusCode = 400 };
            }
            await _business.CreatePagamentoAsync(pagamentoDto.Importo, pagamentoDto.DataPagamento, pagamentoDto.Fk_Ordine, pagamentoDto.Fk_MetodoPagamento, cancellationToken);
            _logger.LogInformation("Pagamento creato con successo!");
            return new JsonResult(new { message = "Pagamento creato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del pagamento.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetPagamentoById")]
    public async Task<ActionResult<PagamentoDto?>> GetPagamentoById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamento = await _business.GetPagamentoByIdAsync(id, cancellationToken);
            if (pagamento == null)
                return new JsonResult(new { error = $"Pagamento con ID '{id}' non trovato." }) { StatusCode = 404 };
            return new JsonResult(pagamento) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del pagamento.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetAllPagamenti")]
    public async Task<ActionResult<List<PagamentoDto>>> GetAllPagamenti(CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamenti = await _business.GetAllPagamentiAsync(cancellationToken);
            if (pagamenti == null || pagamenti.Count == 0)
                return new JsonResult(new { error = "Nessun pagamento trovato." }) { StatusCode = 404 };
            return new JsonResult(pagamenti) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei pagamenti.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetAllOrdiniClientHttp")]
    public async Task<IActionResult> GetAllOrdiniClientHttp(CancellationToken cancellationToken = default)
    {
        try
        {
            var ordini = await _ordiniClientHttp.GetAllOrdiniAsync(cancellationToken);
            if (ordini == null || !ordini.Any())
                return new JsonResult(new { error = "Nessun ordine trovato." }) { StatusCode = 404 };
            return new JsonResult(ordini) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            return new JsonResult(new { error = $"Errore durante il recupero degli ordini: {ex.Message}" }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetOrdineByPagamento")]
    public async Task<IActionResult> GetOrdineByPagamento(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var pagamento = await _business.GetPagamentoByIdAsync(id, cancellationToken);
            if (pagamento == null)
                return new JsonResult(new { error = $"Pagamento con ID '{id}' non trovato." }) { StatusCode = 404 };
            var ordine = await _ordiniClientHttp.GetOrdineByIdAsync(pagamento.Fk_Ordine, cancellationToken);
            if (ordine == null)
                return new JsonResult(new { error = $"Ordine con ID '{pagamento.Fk_Ordine}' non trovato." }) { StatusCode = 404 };
            return new JsonResult(new { Pagamento = pagamento, Ordine = ordine }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei dettagli del pagamento e dell'ordine.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
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
            return new JsonResult(new { message = $"Pagamento con ID '{id}' aggiornato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del pagamento con ID '{id}'.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }


    [HttpDelete(Name = "DeletePagamento")]
    public async Task<ActionResult> DeletePagamento(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeletePagamentoAsync(id, cancellationToken);
            return new JsonResult(new { message = $"Pagamento con ID '{id}' eliminato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del pagamento.");
            return new JsonResult(new { error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }
}
