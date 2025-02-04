using Inventario.Repository.Model;
using Inventario.Shared;

namespace Inventario.Business;

public static class MyMapper
{
    public static ArticoloDto MapToDto(this Articolo articolo) => new()
    {
        Id = articolo.Id,
        Nome = articolo.Nome,
        Descrizione = articolo.Descrizione,
        Prezzo = articolo.Prezzo,
        QuantitaDisponibile = articolo.QuantitaDisponibile,
        CodiceSKU = articolo.CodiceSKU,
        Categoria = articolo.Categoria,
        Fk_fornitore = articolo.Fk_fornitore
    };
    public static FornitoreDto MapToDto(this Fornitore fornitore) => new()
    {
        Id = fornitore.Id,
        Nome = fornitore.Nome,
        Indirizzo = fornitore.Indirizzo,
        Telefono = fornitore.Telefono,
        Email = fornitore.Email
    };
}
