using Microsoft.AspNetCore.Mvc;
using Inventario.Business.Abstractions;
using Inventario.Shared;
using Microsoft.Extensions.Logging;
using Inventario.Repository.Model;

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
    public async Task<ActionResult> CreateFornitore(string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
            return new JsonResult(new { message = "Nome e email sono obbligatori." }) { StatusCode = 400 };
        try
        {
            await _business.CreateFornitoreAsync(nome, indirizzo, telefono, email, cancellationToken);
            return new JsonResult(new { message = "Fornitore creato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la creazione del fornitore.");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "ReadFornitore")]
    public async Task<ActionResult<FornitoreDto?>> ReadFornitore(int id)
    {
        try
        {
            var fornitore = await _business.ReadFornitoreAsync(id);
            if (fornitore == null)
                return new JsonResult(new { message = $"Fornitore con ID '{id}' non trovato." }) { StatusCode = 404 };
            return new JsonResult(fornitore) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante la lettura del fornitore con ID '{id}'.");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "GetAllFornitori")]
    public async Task<ActionResult<List<FornitoreDto>>> GetAllFornitori()
    {
        try
        {
            var fornitori = await _business.GetAllFornitoriAsync();
            if (fornitori == null || !fornitori.Any())
                return new JsonResult(new { message = "Nessun fornitore trovato." }) { StatusCode = 404 };
            return new JsonResult(fornitori) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il recupero della lista dei fornitori.");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpPut(Name = "UpdateFornitore")]
    public async Task<ActionResult> UpdateFornitore(int id, [FromBody] FornitoreDto fornitoreDto, CancellationToken cancellationToken = default)
    {
        if (fornitoreDto == null)
            return new JsonResult(new { message = "Dati fornitore non validi." }) { StatusCode = 400 };
        try
        {
            var fornitoreEsistente = await _business.ReadFornitoreAsync(id, cancellationToken);
            if (fornitoreEsistente == null)
                return new JsonResult(new { message = $"Fornitore con ID '{id}' non trovato." }) { StatusCode = 404 };
            await _business.UpdateFornitoreAsync(id, fornitoreDto.Nome, fornitoreDto.Indirizzo, fornitoreDto.Telefono, fornitoreDto.Email, cancellationToken);
            return new JsonResult(new { message = $"Fornitore con ID '{id}' aggiornato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento del fornitore con ID '{id}'.");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpDelete(Name = "DeleteFornitore")]
    public async Task<ActionResult> DeleteFornitore(int id)
    {
        try
        {
            await _business.DeleteFornitoreAsync(id);
            return new JsonResult(new { message = $"Fornitore con ID '{id}' eliminato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'eliminazione del fornitore con ID '{id}'.");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }
}
