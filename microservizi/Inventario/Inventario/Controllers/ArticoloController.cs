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
    public async Task<ArticoloDto> GetCodiceSku(string codiceSku)
    {
        return await _business.GetSkuAsync(codiceSku);
    }
}
