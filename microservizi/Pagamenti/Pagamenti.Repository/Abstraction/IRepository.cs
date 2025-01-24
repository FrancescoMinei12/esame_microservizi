using Pagamenti.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagamenti.Repository.Abstraction;
public interface IRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
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