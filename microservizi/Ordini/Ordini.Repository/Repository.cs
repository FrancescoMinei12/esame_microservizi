using Microsoft.EntityFrameworkCore;
using Ordini.Repository.Abstraction;
using Ordini.Repository.Model;

namespace Ordini.Repository;

public class Repository(OrdiniDbContext ordiniDbContext) : IRepository
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.SaveChangesAsync(cancellationToken);
    }
    // Clienti
    public async Task<Cliente> CreateClienteAsync(string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {
        Cliente cliente = new Cliente
        {
            Nome = nome,
            Cognome = cognome,
            Email = email,
            Telefono = telefono,
            Indirizzo = indirizzo
        };
        await ordiniDbContext.Clienti.AddAsync(cliente);
        return cliente;
    }
    public async Task<Ordine?> AggiornaTotaleOrdineAsync(int id, decimal nuovoTotale, CancellationToken cancellationToken = default)
    {
        Ordine? ordine = await ordiniDbContext.Ordini.SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (ordine == null)
        {
            throw new ArgumentException("Ordine non trovato.", nameof(id));
        }
        ordine.Totale = nuovoTotale;
        return ordine;
    }
    public async Task<List<Ordine>> GetOrdiniByArticoloIdAsync(int IdArticolo, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Ordini.Where(o => o.Fk_cliente == IdArticolo).ToListAsync(cancellationToken);
    }
    public async Task<Cliente?> ReadClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Clienti
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cliente?> ReadClienteByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
       return await ordiniDbContext.Clienti
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Email == email, cancellationToken);
    }
    public async Task<List<Cliente>> GetAllClientiAsync(CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Clienti.AsNoTracking().ToListAsync(cancellationToken);
    }
    public async Task UpdateClienteAsync(int id, string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {
        Cliente? cliente = await ReadClienteAsync(id, cancellationToken);
        if (cliente == null)
            return;
        cliente.Nome = nome;
        cliente.Cognome = cognome;
        cliente.Email = email;
        cliente.Telefono = telefono;
        cliente.Indirizzo = indirizzo;
    }
    public async Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        Cliente? cliente = await ReadClienteAsync(id, cancellationToken);
        if (cliente == null)
            return;
        ordiniDbContext.Clienti.Remove(cliente);
    }

    // Ordini
    public async Task<Ordine> CreateOrdineAsync(int fk_cliente, decimal totale, CancellationToken cancellationToken = default)
    {
        Ordine ordine = new Ordine
        {
            Fk_cliente = fk_cliente,
            Totale = totale,
            DataOrdine = DateTime.UtcNow
        };
        await ordiniDbContext.Ordini.AddAsync(ordine, cancellationToken);
        return ordine;
    }
    public async Task<Ordine?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Ordini
            .AsNoTracking()
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
    public async Task<List<Ordine>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Ordini
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task UpdateOrdineAsync(int id, decimal totale, int fk_cliente, CancellationToken cancellationToken = default)
    {
        Ordine? ordine = await ReadOrdineAsync(id, cancellationToken);
        if (ordine == null)
            return;
        ordine.Totale = totale;
        ordine.Fk_cliente = fk_cliente;
    }
    public async Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        Ordine? ordine = await ReadOrdineAsync(id, cancellationToken);
        if (ordine == null)
            return;
        ordiniDbContext.Ordini.Remove(ordine);
    }

    // OrdineProdotti
    public async Task<OrdineProdotti> AddProdottoToOrdineAsync(int quantita, int fk_ordine, int fk_prodotto, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = new OrdineProdotti
        {
            Quantita = quantita,
            Fk_ordine = fk_ordine,
            Fk_prodotto = fk_prodotto
        };
        await ordiniDbContext.OrdineProdotti.AddAsync(ordineProdotti, cancellationToken);
        return ordineProdotti;
    }
    public async Task<OrdineProdotti?> ReadOrdiniProdottiAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.OrdineProdotti
            .AsNoTracking()
            .SingleOrDefaultAsync(op => op.Id == id, cancellationToken);
    }
    public async Task<List<OrdineProdotti>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.OrdineProdotti
            .AsNoTracking()
            .Where(op => op.Fk_ordine == fk_ordine)
            .ToListAsync(cancellationToken);
    }
    public async Task UpdateOrdineProdottoAsync(int id, int quantita, int fk_ordine, int fk_prodotto, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = await ReadOrdiniProdottiAsync(id, cancellationToken);
        if (ordineProdotti == null)
            throw new ArgumentException("OrdineProdotti non trovato.", nameof(id));
        ordineProdotti.Quantita = quantita;
        ordineProdotti.Fk_ordine = fk_ordine;
        ordineProdotti.Fk_prodotto = fk_prodotto;
    }
    public async Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = await ReadOrdiniProdottiAsync(id, cancellationToken);
        if (ordineProdotti == null)
        {
            throw new ArgumentException("OrdineProdotti non trovato.", nameof(id));
        }
        ordiniDbContext.OrdineProdotti.Remove(ordineProdotti);
    }
}
