namespace Pagamenti.Business.Abstractions;
public interface IPagamentiService
{
    Task RicalcolaTotalePagamentiAsync(int ordineId, decimal nuovoTotale, CancellationToken cancellationToken);
}
