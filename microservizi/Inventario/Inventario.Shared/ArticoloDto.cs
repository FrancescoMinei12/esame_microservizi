namespace Inventario.Shared;
public class ArticoloDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descrizione { get; set; }
    public decimal Prezzo { get; set; }
    public int QuantitaDisponibile { get; set; }
    public string CodiceSKU { get; set; }
    public string Categoria { get; set; }
    public int Fk_fornitore { get; set; }
}
