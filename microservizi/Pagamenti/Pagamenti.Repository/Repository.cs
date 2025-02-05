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
    public async Task<Pagamento> CreatePagamentoAsync(decimal importo, DateTime dataPagamento, int fk_Ordine, int fk_MetodoPagamento, CancellationToken cancellationToken = default)
    {
        Pagamento pagamento = new Pagamento
        {
            Importo = importo,
            DataPagamento = dataPagamento,
            Fk_Ordine = fk_Ordine,
            Fk_MetodoPagamento = fk_MetodoPagamento
        };
        await pagamentoDbContext.Pagamenti.AddAsync(pagamento, cancellationToken);
        return pagamento;
    }
    public async Task<Pagamento?> ReadPagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await pagamentoDbContext.Pagamenti.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
    public async Task<List<Pagamento>> GetAllPagamentiAsync(CancellationToken cancellationToken = default)
    {
        return await pagamentoDbContext.Pagamenti.AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task UpdatePagamentoAsync(int id, decimal importo, DateTime dataPagamento, int fk_Ordine, int fk_MetodoPagamento, CancellationToken cancellationToken = default)
    {
        Pagamento? pagamento = await ReadPagamentoAsync(id, cancellationToken);
        if (pagamento == null)
        {
            throw new ArgumentException("Pagamento non trovato");
        }
        pagamento.Importo = importo;
        pagamento.DataPagamento = dataPagamento;
        pagamento.Fk_Ordine = fk_Ordine;
        pagamento.Fk_MetodoPagamento = fk_MetodoPagamento;
    }
    public async Task DeletePagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        Pagamento? pagamento = await ReadPagamentoAsync(id, cancellationToken);
        if (pagamento == null)
        {
            throw new ArgumentException("Pagamento non trovato");
        }
        pagamentoDbContext.Pagamenti.Remove(pagamento);
    }

    // MetodiPagamento
    public async Task<MetodoPagamento> CreateMetodoPagamentoAsync(string nome, CancellationToken cancellationToken = default)
    {
        MetodoPagamento metodoPagamento = new MetodoPagamento
        {
            Nome = nome
        };
        await pagamentoDbContext.MetodiPagamento.AddAsync(metodoPagamento, cancellationToken);
        return metodoPagamento;
    }
    public async Task<MetodoPagamento?> ReadMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await pagamentoDbContext.MetodiPagamento.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
    public async Task<List<MetodoPagamento>> GetAllMetodiPagamentoAsync(CancellationToken cancellationToken = default)
    {
        return await pagamentoDbContext.MetodiPagamento.ToListAsync(cancellationToken);
    }
    public async Task UpdateMetodoPagamentoAsync(int id, string nome, CancellationToken cancellationToken = default)
    {
        MetodoPagamento? metodoPagamento = await ReadMetodoPagamentoAsync(id, cancellationToken);
        if (metodoPagamento == null)
        {
            throw new ArgumentException("Metodo di pagamento non trovato");
        }
        metodoPagamento.Nome = nome;
    }
    public async Task DeleteMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        MetodoPagamento? metodoPagamento = await ReadMetodoPagamentoAsync(id,cancellationToken);
        if (metodoPagamento == null)
        {
            throw new ArgumentException("Metodo di pagamento non trovato");
        }
        pagamentoDbContext.MetodiPagamento.Remove(metodoPagamento);
    }
}
