using Inventario.Repository.Abstraction;
using Inventario.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Repository;

public class Repository(InventarioDbContext inventarioDbContext) : IRepository
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await inventarioDbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task CreateArticoloAsync(string nome, string desc, decimal prezzo, int quantita, string SKU, string categoria, CancellationToken cancellationToken)
    {
        Articolo a = new Articolo();
        a.Nome = nome;
        a.Descrizione = desc;
        a.Prezzo = prezzo;
        a.QuantitaDisponibile = quantita;
        a.CodiceSKU = SKU;
        a.Categoria = categoria;
        await inventarioDbContext.Articoli.AddAsync(a, cancellationToken);
    }

    public async Task<Articolo?> ReadArticoloAsync(int id, CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Articoli.Where(a => a.Id == id).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Articolo?> ReadArticoloAsync(string codiceSKU, CancellationToken cancellationToken)
    {
        return await inventarioDbContext.Articoli.Where(a => a.CodiceSKU == codiceSKU).SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Articolo?>> ReadArticoloCategoria(string categoria, CancellationToken cancellationToken = default)
    {
        return await inventarioDbContext.Articoli
            .Where(a => a.Categoria == categoria)
            .ToListAsync(cancellationToken);
    }

    public IQueryable<Articolo> ReadArticoli()
    {
        return inventarioDbContext.Articoli;
    }
}
