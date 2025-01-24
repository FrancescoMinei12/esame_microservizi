namespace Ordini.Shared;
public class OrdineProdottiDto
{
    public int Id { get; set; }
    public int Quantita { get; set; }
    public int Fk_ordine { get; set; }
    public int Fk_prodotto { get; set; }
}
