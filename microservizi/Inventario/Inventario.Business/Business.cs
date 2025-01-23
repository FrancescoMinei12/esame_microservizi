using Inventario.Business.Abstractions;
using Inventario.Repository.Abstraction;
using Inventario.Repository.Model;
using Inventario.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Inventario.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    // Articoli
    public async Task CreateArticoloAsync(string nome, string desc, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        await repository.CreateArticoloAsync(nome, desc, prezzo, quantita, SKU, categoria, fk_fornitore, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ReadArticoloAsync(id, cancellationToken);
        if (articolo == null)
            return null;

        return new ArticoloDto
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
    }

    public async Task<ArticoloDto?> GetSkuAsync(string codiceSKU, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ReadArticoloAsync(codiceSKU, cancellationToken);
        if (articolo == null)
            return null;

        return new ArticoloDto
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
    }

    public async Task<List<ArticoloDto?>> GetCategoriaAsync(string categoria, CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadArticoloCategoria(categoria, cancellationToken);
        var articoliDto = articoli
            .Where(a => a != null)
            .Select(a => new ArticoloDto
            {
                Id = a!.Id,
                Nome = a.Nome,
                Descrizione = a.Descrizione,
                Prezzo = a.Prezzo,
                QuantitaDisponibile = a.QuantitaDisponibile,
                CodiceSKU = a.CodiceSKU,
                Categoria = a.Categoria,
                Fk_fornitore = a.Fk_fornitore
            })
            .ToList();
        return articoliDto;
    }

    public async Task<List<ArticoloDto?>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadArticoloFornitore(id_fornitore, cancellationToken);
        var articoliDto = articoli
            .Where(a => a != null)
            .Select(a => new ArticoloDto
            {
                Id = a!.Id,
                Nome = a.Nome,
                Descrizione = a.Descrizione,
                Prezzo = a.Prezzo,
                QuantitaDisponibile = a.QuantitaDisponibile,
                CodiceSKU = a.CodiceSKU,
                Categoria = a.Categoria,
                Fk_fornitore = a.Fk_fornitore
            })
            .ToList();
        return articoliDto;
    }
    public async Task<List<ArticoloDto>> ReadAllArticoli(CancellationToken cancellationToken = default)
    {
        var articoli = await repository.ReadAllArticoli();
        var articoliDto = articoli
            .Select(a => new ArticoloDto
            {
                Id = a.Id,
                Nome = a.Nome,
                Descrizione = a.Descrizione,
                Prezzo = a.Prezzo,
                QuantitaDisponibile = a.QuantitaDisponibile,
                CodiceSKU = a.CodiceSKU,
                Categoria = a.Categoria,
                Fk_fornitore = a.Fk_fornitore
            }).ToList();
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

        return new FornitoreDto
        {
            Id = fornitore.Id,
            Nome = fornitore.Nome,
            Indirizzo = fornitore.Indirizzo,
            Telefono = fornitore.Telefono,
            Email = fornitore.Email
        };
    }

    public async Task<List<FornitoreDto>> GetAllFornitoriAsync(CancellationToken cancellationToken = default)
    {
        var fornitori = await repository.GetAllFornitoriAsync(cancellationToken);
        return fornitori.Select(f => new FornitoreDto
        {
            Id = f.Id,
            Nome = f.Nome,
            Indirizzo = f.Indirizzo,
            Telefono = f.Telefono,
            Email = f.Email
        }).ToList();
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
