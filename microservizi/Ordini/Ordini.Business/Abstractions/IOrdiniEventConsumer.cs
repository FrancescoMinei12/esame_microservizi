namespace Ordini.Business.Abstractions;
public interface IOrdiniEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}
