using Microsoft.AspNetCore.Mvc;
using Ordini.Business.Abstractions;
using Ordini.Shared;
using Microsoft.Extensions.Logging;

namespace Ordini.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ClientiController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<ClientiController> _logger;

    public ClientiController(IBusiness business, ILogger<ClientiController> logger)
    {
        _business = business;
        _logger = logger;
    }

    [HttpPost(Name = "CreateCliente")]
    public async Task<ActionResult> CreateCliente(string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.CreateClienteAsync(nome, cognome, email, telefono, indirizzo, cancellationToken);
            return Ok("Cliente creato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del cliente.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetClienteById")]
    public async Task<ActionResult<ClienteDto?>> GetClienteById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _business.GetClienteByIdAsync(id, cancellationToken);
            if (cliente == null)
                return NotFound($"Cliente con ID '{id}' non trovato.");

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del cliente.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetClienteByEmail")]
    public async Task<ActionResult<ClienteDto?>> GetClienteByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var cliente = await _business.GetClienteByEmailAsync(email, cancellationToken);
            if (cliente == null)
                return NotFound($"Cliente con email '{email}' non trovato.");

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero del cliente per email.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllClienti")]
    public async Task<ActionResult<List<ClienteDto>>> GetAllClienti(CancellationToken cancellationToken = default)
    {
        try
        {
            var clienti = await _business.GetAllClientiAsync(cancellationToken);
            if (clienti == null || clienti.Count == 0)
                return NotFound("Nessun cliente trovato.");

            return Ok(clienti);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero dei clienti.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpPut(Name = "UpdateCliente")]
    public async Task<ActionResult> UpdateCliente(int id, [FromBody] ClienteDto clienteDto, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.UpdateClienteAsync(id, clienteDto.Nome, clienteDto.Cognome, clienteDto.Email, clienteDto.Telefono, clienteDto.Indirizzo, cancellationToken);
            _logger.LogInformation($"Cliente con ID '{id}' aggiornato correttamente.");
            return Ok($"Cliente con ID '{id}' aggiornato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del cliente con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpDelete(Name = "DeleteCliente")]
    public async Task<ActionResult> DeleteCliente(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.DeleteClienteAsync(id, cancellationToken);
            return Ok($"Cliente con ID '{id}' eliminato con successo!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante l'eliminazione del cliente.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
