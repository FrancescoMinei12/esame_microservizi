namespace Ordini.Shared;
public class OrdineDto
{
    public int Id { get; set; }
    public DateTime DataOrdine { get; set; }
    public decimal Totale { get; set; }
    public int Fk_cliente { get; set; }
}
