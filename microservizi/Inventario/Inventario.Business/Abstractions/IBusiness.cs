using Inventario.Shared;

namespace Inventario.Business.Abstractions;

public interface IBusiness
{
    Task CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
    Task<List<ArticoloDto?>> GetCategoriaAsync(string categoria, CancellationToken cancellationToken = default);
}
