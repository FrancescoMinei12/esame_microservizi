using Inventario.Shared;
namespace Inventario.ClientHttp.Abstraction;
public interface IInventarioClientHttp
{
    Task<string?> CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ScaricaQuantitaAsync(int prodottoId, int quantita, CancellationToken cancellationToken = default);
}