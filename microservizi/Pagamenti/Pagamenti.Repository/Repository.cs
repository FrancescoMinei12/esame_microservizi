using Pagamenti.Repository.Abstraction;
using Pagamenti.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Pagamenti.Repository;
public class Repository(PagamentiDbContext pagamentoDbContext) : IRepository
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await pagamentoDbContext.SaveChangesAsync(cancellationToken);
    }
    // Pagamenti
    Task<Pagamento> CreatePagamentoAsync(decimal importo, DateTime dataPagamento, int fk_Ordine, int fk_MetodoPagamento, CancellationToken cancellationToken = default);
    Task<Pagamento?> ReadPagamentoAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Pagamento>> GetAllPagamentiAsync(CancellationToken cancellationToken = default);
    Task UpdatePagamentoAsync(int id, decimal importo, DateTime dataPagamento, int fk_Ordine, int fk_MetodoPagamento, CancellationToken cancellationToken = default);
    Task DeletePagamentoAsync(int id, CancellationToken cancellationToken = default);

    // MetodiPagamento
    Task<MetodoPagamento> CreateMetodoPagamentoAsync(string nome, CancellationToken cancellationToken = default);
    Task<MetodoPagamento?> ReadMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default);
    Task<List<MetodoPagamento>> GetAllMetodiPagamentoAsync(CancellationToken cancellationToken = default);
    Task UpdateMetodoPagamentoAsync(int id, string nome, CancellationToken cancellationToken = default);
    Task DeleteMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default);
}
