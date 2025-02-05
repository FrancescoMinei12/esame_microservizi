namespace Ordini.Business.Abstractions;
public interface IOutboxProcessor
{
    Task ProcessOutboxAsync();
}
