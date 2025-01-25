using Pagamenti.Shared;

namespace Pagamenti.Business.Abstractions;

public interface IBusiness
{
    // Pagamenti
    Task CreatePagamentoAsync(decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default);
    Task<PagamentoDto?> GetPagamentoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<PagamentoDto>> GetAllPagamentiAsync(CancellationToken cancellationToken = default);
    Task UpdatePagamentoAsync(int id, decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default);
    Task DeletePagamentoAsync(int id, CancellationToken cancellationToken = default);

    // MetodiPagamento
    Task CreateMetodoPagamentoAsync(string nome, CancellationToken cancellationToken = default);
    Task<MetodoPagamentoDto?> GetMetodoPagamentoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<MetodoPagamentoDto>> GetAllMetodiPagamentoAsync(CancellationToken cancellationToken = default);
    Task UpdateMetodoPagamentoAsync(int id, string nome, CancellationToken cancellationToken = default);
    Task DeleteMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default);
}
