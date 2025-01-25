using Microsoft.AspNetCore.Mvc;
using Pagamenti.Business.Abstractions;
using Pagamenti.Shared;
using Microsoft.Extensions.Logging;

namespace Pagamenti.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class MetodiPagamentoController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<MetodiPagamentoController> _logger;

    public MetodiPagamentoController(IBusiness business, ILogger<MetodiPagamentoController> logger)
    {
        _business = business;
        _logger = logger;
    }
    [HttpPost(Name = "CreateMetodoPagamento")]
    public async Task<ActionResult> CreateMetodoPagamento(string nome, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreateMetodoPagamentoAsync(nome, cancellationToken);
            return Ok("Metodo di pagamento creato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del metodo di pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetMetodoPagamentoById")]
    public async Task<ActionResult<MetodoPagamentoDto?>> GetMetodoPagamentoById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var metodoPagamento = await _business.GetMetodoPagamentoByIdAsync(id, cancellationToken);
            if (metodoPagamento == null)
                return NotFound($"Metodo di pagamento con ID '{id}' non trovato.");

            return Ok(metodoPagamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del metodo di pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllMetodiPagamento")]
    public async Task<ActionResult<List<MetodoPagamentoDto>>> GetAllMetodiPagamento(CancellationToken cancellationToken = default)
    {
        try
        {
            var metodiPagamento = await _business.GetAllMetodiPagamentoAsync(cancellationToken);
            if (metodiPagamento == null || metodiPagamento.Count == 0)
                return NotFound("Nessun metodo di pagamento trovato.");

            return Ok(metodiPagamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei metodi di pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpPut(Name = "UpdateMetodoPagamento")]
    public async Task<ActionResult> UpdateMetodoPagamento(int id, [FromBody] MetodoPagamentoDto metodoPagamentoDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.UpdateMetodoPagamentoAsync(id, metodoPagamentoDto.Nome, cancellationToken);
            _logger.LogInformation($"Metodo di pagamento con ID '{id}' aggiornato correttamente.");
            return Ok($"Metodo di pagamento con ID '{id}' aggiornato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del metodo di pagamento con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpDelete(Name = "DeleteMetodoPagamento")]
    public async Task<ActionResult> DeleteMetodoPagamento(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeleteMetodoPagamentoAsync(id, cancellationToken);
            return Ok($"Metodo di pagamento con ID '{id}' eliminato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del metodo di pagamento.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
