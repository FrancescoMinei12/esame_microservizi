namespace Ordini.Business.Abstractions;
public interface IOrdineService
{
    Task RicalcolaTotaleOrdiniAsync(int articoloId, CancellationToken cancellationToken);
}
