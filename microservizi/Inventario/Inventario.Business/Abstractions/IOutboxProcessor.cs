namespace Inventario.Business.Abstractions;
public interface IOutboxProcessor
{
    Task ProcessOutboxAsync();
}
