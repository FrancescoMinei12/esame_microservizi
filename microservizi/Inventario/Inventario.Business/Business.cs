using Inventario.Business.Abstractions;
using Inventario.Repository.Abstraction;
using Inventario.Shared;
using Microsoft.Extensions.Logging;

namespace Inventario.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    // Articoli
    public async Task CreateArticoloAsync(string nome, string desc, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        await repository.CreateArticoloAsync(nome, desc, prezzo, quantita, SKU, categoria, fk_fornitore, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<ArticoloDto?> ModificaPrezzoArticoloAsync(int id, decimal nuovoPrezzo, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ModificaPrezzoArticoloAsync(id, nuovoPrezzo, cancellationToken);
        if (articolo == null)
            return null;
        await repository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Prezzo modificato per l'articolo con ID '{id}'. Nuovo prezzo: {nuovoPrezzo}.", articolo.Id, articolo.Prezzo);
        return articolo.MapToDto();
    }
    public async Task<ArticoloDto?> ScaricaQuantitaAsync(int articoloId, int quantita, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ScaricaQuantitaAsync(articoloId, quantita, cancellationToken);
        if (articolo == null)
            return null;
        await repository.SaveChangesAsync();
        return articolo.MapToDto();
    }
    public async Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ReadArticoloAsync(id, cancellationToken);
        if (articolo == null)
            return null;
        return articolo.MapToDto();
    }

    public async Task<ArticoloDto?> GetSkuAsync(string codiceSKU, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ReadArticoloAsync(codiceSKU, cancellationToken);
        if (articolo == null)
            return null;
        return articolo.MapToDto();
    }

    public async Task<List<ArticoloDto?>> GetCategoriaAsync(string categoria, CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadArticoloCategoria(categoria, cancellationToken);
        var articoliDto = articoli
            .Where(a => a != null)
            .Select(a => a.MapToDto())
            .ToList();
        return articoliDto;
    }

    public async Task<List<ArticoloDto?>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadArticoloFornitore(id_fornitore, cancellationToken);
        var articoliDto = articoli
            .Where(a => a != null)
            .Select(a => a.MapToDto())
            .ToList();
        return articoliDto;
    }
    public async Task<List<ArticoloDto>> ReadAllArticoli(CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadAllArticoli();
        var articoliDto = articoli
            .Select(a => a.MapToDto())
            .ToList();
        return articoliDto;
    }
    public async Task UpdateArticoloAsync(int id, string nome, string descrizione, decimal prezzo, int quantitaDisponibile, string codiceSKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        await repository.UpdateArticoloAsync(id, nome, descrizione, prezzo, quantitaDisponibile, codiceSKU, categoria, fk_fornitore, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteArticoloAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteArticoloAsync(id);
        await repository.SaveChangesAsync(cancellationToken);
    }

    // Fornitori
    public async Task CreateFornitoreAsync(string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default)
    {
        await repository.CreateFornitoreAsync(nome, indirizzo, telefono, email, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<FornitoreDto?> ReadFornitoreAsync(int id, CancellationToken cancellationToken = default)
    {
        var fornitore = await repository.ReadFornitoreAsync(id, cancellationToken);
        if (fornitore == null)
            return null;

        return fornitore.MapToDto();
    }

    public async Task<List<FornitoreDto>> GetAllFornitoriAsync(CancellationToken cancellationToken = default)
    {
        var fornitori = await repository.GetAllFornitoriAsync(cancellationToken);
        return fornitori.Select(f => f.MapToDto()).ToList();
    }

    public async Task UpdateFornitoreAsync(int id, string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default)
    {
        await repository.UpdateFornitoreAsync(id, nome, indirizzo, telefono, email, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFornitoreAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteFornitoreAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}