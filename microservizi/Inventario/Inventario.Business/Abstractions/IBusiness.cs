using Inventario.Shared;

namespace Inventario.Business.Abstractions;

internal interface IBusiness
{
    Task CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
}
