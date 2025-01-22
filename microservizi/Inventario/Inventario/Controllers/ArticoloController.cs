using Microsoft.AspNetCore.Mvc;
using Inventario.Business.Abstractions;
using Inventario.Shared;
using Inventario.Repository.Model;

namespace Inventario.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ArticoloController : ControllerBase
{
    private readonly IBusiness _business;
    private readonly ILogger<ArticoloController> _logger;
    public ArticoloController(IBusiness business, ILogger<ArticoloController> logger)
    {
        _business = business;
        _logger = logger;
    }

    [HttpPost(Name = "CreateArticolo")]
    public async Task<ActionResult> CreateArticolo(string nome, string descrizione, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        await _business.CreateArticoloAsync(nome, descrizione, prezzo, quantita, SKU, categoria, fk_fornitore, cancellationToken);
        return Ok("Articolo creato correttamente!");
    }

    [HttpGet(Name = "ReadArticolo")]
    public async Task<ActionResult<ArticoloDto?>> ReadArticolo(int id)
    {
        var articolo = await _business.ReadArticoloAsync(id);
        return new JsonResult(articolo);
    }

    [HttpGet(Name = "GetCodiceSku")]
    public async Task<ActionResult<ArticoloDto?>> GetCodiceSku(string codiceSku)
    {
        var articolo = await _business.GetSkuAsync(codiceSku);
        return new JsonResult(articolo);
    }

    [HttpGet(Name = "GetCategoria")]
    public async Task<ActionResult<List<ArticoloDto>>> GetCategoria(string categoria)
    {
        var articoli = await _business.GetCategoriaAsync(categoria);
        return new JsonResult(articoli);
    }

    [HttpGet(Name = "ReadArticoloFornitore")]
    public async Task<ActionResult<List<ArticoloDto>>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default)
    {
        var articoli = await _business.ReadArticoloFornitore(id_fornitore, cancellationToken);
        return Ok(articoli);
    }


    [HttpPut(Name = "UpdateArticolo")]
    public async Task<ActionResult> UpdateArticolo(int id, [FromBody] ArticoloDto articoloDto, CancellationToken cancellationToken = default)
    {
        if (articoloDto == null)
        {
            return BadRequest("Dati articolo non validi.");
        }
        try
        {
            await _business.UpdateArticoloAsync(
                id,
                articoloDto.Nome,
                articoloDto.Descrizione,
                articoloDto.Prezzo,
                articoloDto.QuantitaDisponibile,
                articoloDto.CodiceSKU,
                articoloDto.Categoria,
                articoloDto.Fk_fornitore,
                cancellationToken
            );
            return Ok($"Articolo con ID '{id}' aggiornato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento dell'articolo con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }


    [HttpDelete(Name = "DeleteArticolo")]
    public async Task<ActionResult> DeleteArticolo(int id)
    {
        try
        {
            await _business.DeleteArticoloAsync(id);
            return Ok($"Articolo con ID '{id}' eliminato correttamente!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'eliminazione dell'articolo con ID '{id}'.");
            return StatusCode(500, "Errore interno del server.");
        }
    }
}
