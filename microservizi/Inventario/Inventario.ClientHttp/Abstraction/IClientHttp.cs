using Inventario.Shared;
namespace Inventario.ClientHttp.Abstraction;
internal interface IClientHttp
{
    Task<string?> CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
}