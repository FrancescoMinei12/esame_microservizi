namespace Ordini.Repository.Model;
public class Ordine
{
    public int Id { get; set; }
    public DateTime DataOrdine { get; set; }
    public decimal Totale { get; set; }
    public int Fk_cliente { get; set; }
    public Cliente Cliente { get; set; }
    public List<OrdineProdotti> OrdiniProdotti { get; set; }
}
