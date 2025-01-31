using Inventario.Repository.Abstraction;
using Inventario.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading;

namespace Inventario.Repository;

public class Repository(InventarioDbContext inventarioDbContext) : IRepository
{
    // Articolo
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await inventarioDbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<Articolo> CreateArticoloAsync(string nome, string descrizione, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        Articolo articolo = new Articolo
        {
            Nome = nome,
            Descrizione = descrizione,
            Prezzo = prezzo,
            QuantitaDisponibile = quantita,
            CodiceSKU = SKU,
            Categoria = categoria,
            DataInserimento = DateTime.Now,
            Fk_fornitore = fk_fornitore
        };
        await inventarioDbContext.Articoli.AddAsync(articolo, cancellationToken);
        return articolo;
    }

    public async Task<Articolo?> ReadArticoloAsync(int id, CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Articoli
            .Where(a => a.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Articolo?> ReadArticoloAsync(string codiceSKU, CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Articoli
            .Where(a => a.CodiceSKU == codiceSKU)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Articolo>> ReadArticoloCategoria(string categoria, CancellationToken cancellationToken = default)
    {
        return await inventarioDbContext.Articoli
            .Where(a => a.Categoria == categoria)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<Articolo?>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default)
    {
        var articoli = await inventarioDbContext.Articoli
        .Where(a => a.Fk_fornitore == id_fornitore)
        .ToListAsync(cancellationToken);

        return articoli;
    }

    public async Task<List<Articolo>> ReadAllArticoli()
    {
        return await inventarioDbContext.Articoli.ToListAsync();
    }

    public async Task UpdateArticoloAsync(int id, string nome, string descrizione, decimal prezzo, int quantitaDisponibile, string codiceSKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default)
    {
        Articolo? articolo = await ReadArticoloAsync(id, cancellationToken);
        if (articolo == null)
            return;
        articolo.Nome = nome;
        articolo.Descrizione = descrizione;
        articolo.Prezzo = prezzo;
        articolo.QuantitaDisponibile = quantitaDisponibile;
        articolo.CodiceSKU = codiceSKU;
        articolo.Categoria = categoria;
        articolo.Fk_fornitore = fk_fornitore;

        var outBoxMessage = new TransactionalOutbox
        {
            Tabella = "Articoli",
            Messaggio = JsonSerializer.Serialize(new
            {
                Event = "InventarioAggiornato",
                ArticoloId = articolo.Id,
                Nome = articolo.Nome,
                Quantita = articolo.QuantitaDisponibile,
                Prezzo = articolo.Prezzo,
                Timestamp = DateTime.UtcNow
            })
        };
        await inventarioDbContext.TransactionalOutboxes.AddAsync(outBoxMessage);
    }

    public async Task DeleteArticoloAsync(int id, CancellationToken cancellationToken)
    {
        Articolo? articolo = await ReadArticoloAsync(id, cancellationToken);
        if (articolo == null) return;
        inventarioDbContext.Articoli.Remove(articolo);
    }

    // Fornitori
    public async Task<Fornitore> CreateFornitoreAsync(string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default)
    {
        Fornitore fornitore = new Fornitore
        {
            Nome = nome,
            Indirizzo = indirizzo,
            Telefono = telefono,
            Email = email,
        };
        await inventarioDbContext.Fornitori.AddAsync(fornitore);
        return fornitore;
    }

    public async Task<Fornitore?> ReadFornitoreAsync(int id, CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Fornitori
            .Where(f => f.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Fornitore>> GetAllFornitoriAsync(CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Fornitori
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateFornitoreAsync(int id, string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default)
    {
        Fornitore? fornitore = await ReadFornitoreAsync(id, cancellationToken);
        if (fornitore == null) return;
        fornitore.Nome = nome;
        fornitore.Indirizzo = indirizzo;
        fornitore.Telefono = telefono;
        fornitore.Email = email;
    }

    public async Task DeleteFornitoreAsync(int id, CancellationToken cancellationToken = default)
    {
        Fornitore? fornitore = await ReadFornitoreAsync(id, cancellationToken);
        if (fornitore == null)
            return;
        inventarioDbContext.Fornitori.Remove(fornitore);
    }
}
