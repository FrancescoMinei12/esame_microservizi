namespace Pagamenti.Business.Abstractions;
public interface IPagamentiEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}