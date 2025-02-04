using Ordini.Repository.Model;
using Ordini.Shared;

namespace Ordini.Business;
public static class MyMapper
{
    public static ClienteDto MapToDto(this Cliente cliente) =>new()
    {
        Id = cliente.Id,
        Nome = cliente.Nome,
        Cognome = cliente.Cognome,
        Email = cliente.Email,
        Telefono = cliente.Telefono,
        Indirizzo = cliente.Indirizzo
    };
    public static OrdineDto MapToDto(this Ordine ordine) => new()
    {
        Id = ordine.Id,
        DataOrdine = ordine.DataOrdine,
        Totale = ordine.Totale,
        Fk_cliente = ordine.Fk_cliente
    };
    public static OrdineProdottiDto MapToDto(this OrdineProdotti ordineProdotti) => new()
    {
        Id = ordineProdotti.Id,
        Quantita = ordineProdotti.Quantita,
        Fk_ordine = ordineProdotti.Fk_ordine,
        Fk_prodotto = ordineProdotti.Fk_prodotto
    };
}
