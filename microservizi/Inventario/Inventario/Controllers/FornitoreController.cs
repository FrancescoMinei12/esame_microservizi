using Microsoft.AspNetCore.Mvc;
using Inventario.Business.Abstractions;
using Inventario.Shared;
using Microsoft.Extensions.Logging;

namespace Inventario.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FornitoreController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<FornitoreController> _logger;

    public FornitoreController(IBusiness business, ILogger<FornitoreController> logger)
    {
        _business = business;
        _logger = logger;
    }

    [HttpPost(Name = "CreateFornitore")]
    public async Task<ActionResult> CreateFornitore(
        string nome,
        string indirizzo,
        string telefono,
        string email)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Nome e email sono obbligatori.");
        }

        try
        {
            await _business.CreateFornitoreAsync(nome, indirizzo, telefono, email);
            return Ok("Fornitore creato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del fornitore.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "ReadFornitore")]
    public async Task<ActionResult<FornitoreDto?>> ReadFornitore(int id)
    {
        try
        {
            var fornitore = await _business.ReadFornitoreAsync(id);
            if (fornitore == null)
            {
                return NotFound($"Fornitore con ID '{id}' non trovato.");
            }

            return Ok(fornitore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante la lettura del fornitore con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpGet(Name = "GetAllFornitori")]
    public async Task<ActionResult<List<FornitoreDto>>> GetAllFornitori()
    {
        try
        {
            var fornitori = await _business.GetAllFornitoriAsync();
            if (fornitori == null || !fornitori.Any())
            {
                return NotFound("Nessun fornitore trovato.");
            }

            return Ok(fornitori);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero della lista dei fornitori.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpPut(Name = "UpdateFornitore")]
    public async Task<ActionResult> UpdateFornitore(
        int id,
        string nome,
        string indirizzo,
        string telefono,
        string email)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Nome e email sono obbligatori.");
        }

        try
        {
            await _business.UpdateFornitoreAsync(id, nome, indirizzo, telefono, email);
            return Ok($"Fornitore con ID '{id}' aggiornato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del fornitore con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }

    [HttpDelete(Name = "DeleteFornitore")]
    public async Task<ActionResult> DeleteFornitore(int id)
    {
        try
        {
            await _business.DeleteFornitoreAsync(id);
            return Ok($"Fornitore con ID '{id}' eliminato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'eliminazione del fornitore con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
