using Inventario.Shared;
namespace Inventario.ClientHttp.Abstraction;
public interface IClientHttp
{
    Task<string?> CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
}