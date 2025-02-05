using Pagamenti.Repository.Abstraction;
using Pagamenti.Business.Abstractions;
using Microsoft.Extensions.Logging;
using Pagamenti.Shared;

namespace Pagamenti.Business;
public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    // Pagamenti
    public async Task CreatePagamentoAsync(decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default)
    {
        await repository.CreatePagamentoAsync(importo, dataPagamento, fkOrdine, fkMetodoPagamento, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Pagamento creato con successo.");
    }
    public async Task<PagamentoDto?> GetPagamentoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pagamento = await repository.ReadPagamentoAsync(id, cancellationToken);
        if (pagamento == null)
            return null;
        
        logger.LogInformation("Pagamento trovato: {@pagamento}", pagamento);
        return pagamento.MapToDto();
    }
    public async Task<List<PagamentoDto>> GetAllPagamentiAsync(CancellationToken cancellationToken = default)
    {
        var pagamenti = await repository.GetAllPagamentiAsync(cancellationToken);
        logger.LogInformation("Trovati {Count} pagamenti.", pagamenti.Count);
        return pagamenti.Select(p => p.MapToDto()).ToList();
    }
    public async Task UpdatePagamentoAsync(int id, decimal importo, DateTime dataPagamento, int fkOrdine, int fkMetodoPagamento, CancellationToken cancellationToken = default)
    {
        await repository.UpdatePagamentoAsync(id, importo, dataPagamento, fkOrdine, fkMetodoPagamento, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Pagamento aggiornato con successo.");
    }
    public async Task DeletePagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeletePagamentoAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Pagamento eliminato con successo.");
    }

    // MetodiPagamento
    public async Task CreateMetodoPagamentoAsync(string nome, CancellationToken cancellationToken = default)
    {
        await repository.CreateMetodoPagamentoAsync(nome, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Metodo di pagamento creato con successo.");
    }
    public async Task<MetodoPagamentoDto?> GetMetodoPagamentoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var metodoPagamento = await repository.ReadMetodoPagamentoAsync(id, cancellationToken);
        if (metodoPagamento == null)
            return null;
        logger.LogInformation("Metodo di pagamento trovato: {@metodoPagamento}", metodoPagamento);
        return metodoPagamento.MapToDto();
    }
    public async Task<List<MetodoPagamentoDto>> GetAllMetodiPagamentoAsync(CancellationToken cancellationToken = default)
    {
        var metodiPagamento = await repository.GetAllMetodiPagamentoAsync(cancellationToken);
        logger.LogInformation("Trovati {Count} metodi di pagamento.", metodiPagamento.Count);
        return metodiPagamento.Select(m => m.MapToDto()).ToList();
    }
    public async Task UpdateMetodoPagamentoAsync(int id, string nome, CancellationToken cancellationToken = default)
    {
        await repository.UpdateMetodoPagamentoAsync(id, nome, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Metodo di pagamento aggiornato con successo.");
    }
    public async Task DeleteMetodoPagamentoAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteMetodoPagamentoAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Metodo di pagamento eliminato con successo.");
    }
}
