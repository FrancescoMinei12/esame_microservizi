using Inventario.Business.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Inventario.Shared;

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
        return new JsonResult(new { message = "Articolo creato correttamente!" }) { StatusCode = 200 };
    }

    [HttpPost(Name = "ModificaPrezzoArticolo")]
    public async Task<ActionResult> ModificaPrezzoArticolo(int id, [FromBody] decimal nuovoPrezzo, CancellationToken cancellationToken = default)
    {
        try
        {
            await _business.ModificaPrezzoArticoloAsync(id, nuovoPrezzo, cancellationToken);
            return new JsonResult(new { message = "Prezzo dell'articolo modificato con successo!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la modifica del prezzo");
            return new JsonResult(new { message = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "ScaricaQuantita")]
    public async Task<ActionResult<ArticoloDto?>> ScaricaQuantita(int prodottoId, int quantita, CancellationToken cancellationToken = default)
    {
        if (quantita <= 0)
        {
            _logger.LogWarning("La quantita da scalare deve essere maggiore di zero.");
            return new JsonResult(new { Error = "La quantita da scalare deve essere maggiore di zero." }) { StatusCode = 400 };
        }
        try
        {
            var articoloDto = await _business.ScaricaQuantitaAsync(prodottoId, quantita, cancellationToken);
            if (articoloDto == null)
            {
                _logger.LogWarning($"Articolo con ID '{prodottoId}' non trovato.");
                return new JsonResult(new { Error = $"Articolo con ID '{prodottoId}' non trovato." }) { StatusCode = 404 };
            }
            return new JsonResult(articoloDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante la scalatura della quantita per il prodotto con ID '{prodottoId}'.");
            return new JsonResult(new { Error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpGet(Name = "ReadArticolo")]
    public async Task<ActionResult<ArticoloDto?>> ReadArticolo(int id)
    {
        var articolo = await _business.ReadArticoloAsync(id);
        if (articolo == null)
            return new JsonResult(new { Error = $"Articolo con ID '{id}' non trovato." }) { StatusCode = 404 };
        return new JsonResult(articolo);
    }

    [HttpGet(Name = "GetCodiceSku")]
    public async Task<ActionResult<ArticoloDto?>> GetCodiceSku(string codiceSku)
    {
        var articolo = await _business.GetSkuAsync(codiceSku);
        if (articolo == null)
            return new JsonResult(new { Error = $"Articolo con il codice '{codiceSku}' non trovato." }) { StatusCode = 404 };
        return new JsonResult(articolo) { StatusCode = 200 };
    }

    [HttpGet(Name = "GetCategoria")]
    public async Task<ActionResult<List<ArticoloDto>>> GetCategoria(string categoria)
    {
        var articoli = await _business.GetCategoriaAsync(categoria);
        if (articoli.Count() == 0)
            return new JsonResult(new { Error = $"Articoli con categoria '{categoria}' non trovati." }) { StatusCode = 404 };
        return new JsonResult(articoli) { StatusCode = 200 };
    }

    [HttpGet(Name = "ReadArticoloFornitore")]
    public async Task<ActionResult<List<ArticoloDto>>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default)
    {
        var articoli = await _business.ReadArticoloFornitore(id_fornitore, cancellationToken);
        if (articoli.Count() == 0)
            return new JsonResult(new { Error = $"Articoli con fornitore '{id_fornitore}' non trovati." }) { StatusCode = 404 };
        return new JsonResult(articoli) { StatusCode = 200 };
    }

    [HttpGet(Name = "GetAllArticoli")]
    public async Task<ActionResult<List<ArticoloDto>>> GetAllArticoli(CancellationToken cancellationToken = default)
    {
        var articoli = await _business.ReadAllArticoli();
        return new JsonResult(articoli) { StatusCode = 200 };
    }

    [HttpPut(Name = "UpdateArticolo")]
    public async Task<ActionResult> UpdateArticolo(int id, [FromBody] ArticoloDto articoloDto, CancellationToken cancellationToken = default)
    {
        if (articoloDto == null)
            return new JsonResult(new { Error = "Dati articolo non validi." }) { StatusCode = 400 };
        try
        {
            await _business.UpdateArticoloAsync(id, articoloDto.Nome, articoloDto.Descrizione, articoloDto.Prezzo, articoloDto.QuantitaDisponibile, articoloDto.CodiceSKU, articoloDto.Categoria, articoloDto.Fk_fornitore, cancellationToken);
            return new JsonResult(new { Message = $"Articolo con ID '{id}' aggiornato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'aggiornamento dell'articolo con ID '{id}'.");
            return new JsonResult(new { Error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }

    [HttpDelete(Name = "DeleteArticolo")]
    public async Task<ActionResult> DeleteArticolo(int id)
    {
        try
        {
            await _business.DeleteArticoloAsync(id);
            return new JsonResult(new { Message = $"Articolo con ID '{id}' eliminato correttamente!" }) { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore durante l'eliminazione dell'articolo con ID '{id}'.");
            return new JsonResult(new { Error = "Errore interno del server." }) { StatusCode = 500 };
        }
    }
}
