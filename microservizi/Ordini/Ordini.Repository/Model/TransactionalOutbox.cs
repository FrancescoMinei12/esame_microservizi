namespace Ordini.Repository.Model;
public class TransactionalOutbox
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool Processed { get; set; }
}