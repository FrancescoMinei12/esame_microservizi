namespace Ordini.Repository.Model;

public class OrdineProdotti
{
    public int Id { get; set; }
    public int Quantita { get; set; }
    public int Fk_ordine { get; set; }
    public int Fk_prodotto { get; set; }
    public Ordine Ordine { get; set; }
}
