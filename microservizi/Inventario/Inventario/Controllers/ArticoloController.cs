using Microsoft.AspNetCore.Mvc;
using Inventario.Business.Abstractions;
using Inventario.Shared;
using System.Runtime.InteropServices;

namespace Inventario.Api.Controllers;

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
    public async Task<ActionResult> CreateArticolo(ArticoloDto articoloDto)
    {
        await _business.CreateArticoloAsync(articoloDto);
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
        if (articoli == null || !articoli.Any())
        {
            return NotFound($"Nessun articolo trovato per la categoria '{categoria}'.");
        }
        return Ok(articoli);
    }
}
