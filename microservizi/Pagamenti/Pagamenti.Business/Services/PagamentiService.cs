using Microsoft.Extensions.Logging;
using Pagamenti.Business.Abstractions;
using Pagamenti.Repository.Abstraction;
using Pagamenti.Repository.Model;

namespace Pagamenti.Business.Services;
public class PagamentiService : IPagamentiService
{
    private readonly IRepository _repository;
    private readonly ILogger<PagamentiService> _logger;
    public PagamentiService(IRepository repository, ILogger<PagamentiService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task RicalcolaTotalePagamentiAsync(int ordineId, decimal nuovoTotale, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Ricalcolo totale pagamenti per ordine {OrdineId} con nuovo totale {NuovoTotale}", ordineId, nuovoTotale);
        var pagamento = await _repository.GetPagamentoByOrdine(ordineId, cancellationToken);
        if (pagamento == null)
            return;
        await _repository.AggiornaTotalePagamentoAsync(pagamento.Id, nuovoTotale, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
