namespace Ordini.Business.Abstractions;
public interface IOrdineService
{
    Task RicalcolaTotaleOrdiniAsync(int articoloId, decimal nuovoPrezzo, CancellationToken cancellationToken);
}
