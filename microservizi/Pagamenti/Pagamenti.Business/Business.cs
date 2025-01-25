using Pagamenti.Business.Abstractions;
using Pagamenti.Repository.Abstraction;
using Pagamenti.Shared;
using Microsoft.Extensions.Logging;
using Pagamenti.Repository.Model;
namespace Pagamenti.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    // Pagamenti
    public async Task CreatePagamentoAsync(decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default)
    {
        await repository.CreatePagamentoAsync(importo, dataPagamento, fkOrdine, fkMetodoPagamento, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<PagamentoDto?> GetPagamentoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pagamento = await repository.ReadPagamentoAsync(id, cancellationToken);
        if (pagamento == null)
        {
            return null;
        }
        return new PagamentoDto
        {
            Id = pagamento.Id,
            Importo = pagamento.Importo,
            DataPagamento = pagamento.DataPagamento,
            Fk_Ordine = pagamento.Fk_Ordine,
            Fk_MetodoPagamento = pagamento.Fk_MetodoPagamento
        };
    }
    public async Task<List<PagamentoDto>> GetAllPagamentiAsync(CancellationToken cancellationToken = default)
    {
        var pagamenti = await repository.GetAllPagamentiAsync(cancellationToken);
        return pagamenti.Select(p => new PagamentoDto
        {
            Id = p.Id,
            Importo = p.Importo,
            DataPagamento = p.DataPagamento,
            Fk_Ordine = p.Fk_Ordine,
            Fk_MetodoPagamento = p.Fk_MetodoPagamento
        }).ToList();
    }
    public async Task UpdatePagamentoAsync(int id, decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default)
    {
        await repository.UpdatePagamentoAsync(id, importo, dataPagamento, fkOrdine, fkMetodoPagamento, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeletePagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeletePagamentoAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    // MetodiPagamento
    public async Task CreateMetodoPagamentoAsync(string nome, CancellationToken cancellationToken = default)
    {
        await repository.CreateMetodoPagamentoAsync(nome, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<MetodoPagamentoDto?> GetMetodoPagamentoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var metodoPagamento = await repository.ReadMetodoPagamentoAsync(id, cancellationToken);
        if (metodoPagamento == null)
        {
            return null;
        }
        return new MetodoPagamentoDto
        {
            Id = metodoPagamento.Id,
            Nome = metodoPagamento.Nome
        };
    }
    public async Task<List<MetodoPagamentoDto>> GetAllMetodiPagamentoAsync(CancellationToken cancellationToken = default)
    {
        var metodiPagamento = await repository.GetAllMetodiPagamentoAsync(cancellationToken);
        return metodiPagamento.Select(m => new MetodoPagamentoDto
        {
            Id = m.Id,
            Nome = m.Nome
        }).ToList();
    }
    public async Task UpdateMetodoPagamentoAsync(int id, string nome, CancellationToken cancellationToken = default)
    {
        await repository.UpdateMetodoPagamentoAsync(id, nome, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteMetodoPagamentoAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
