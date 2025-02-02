namespace Inventario.Repository.Model;
public class TransactionalOutbox
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Processed { get; set; }
}