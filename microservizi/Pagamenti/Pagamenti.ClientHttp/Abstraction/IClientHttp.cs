using Pagamenti.Shared;

namespace Pagamenti.ClientHttp.Abstraction;

public interface IClientHttp
{
    Task<string?> CreatePagamentoAsync(PagamentoDto pagamento, CancellationToken cancellationToken);
    Task<PagamentoDto?> GetPagamentoAsync(int id, CancellationToken cancellationToken);
}
