using Microsoft.EntityFrameworkCore;
using Ordini.Repository.Abstraction;
using Ordini.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public async Task<Cliente?> ReadClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        Cliente? cliente = await ordiniDbContext.Clienti.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return cliente;
    }

    public async Task<Cliente?> ReadClienteByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        Cliente? cliente = await ordiniDbContext.Clienti.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        return cliente;
    }
    public async Task<List<Cliente>> GetAllClientiAsync(CancellationToken cancellationToken = default)
    {
        List<Cliente> clienti = await ordiniDbContext.Clienti.ToListAsync(cancellationToken);
        return clienti;
    }
    public async Task UpdateClienteAsync(int id, string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {

        Cliente? cliente = await ordiniDbContext.Clienti.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (cliente == null)
        {
            return;
        }
        cliente.Nome = nome;
        cliente.Cognome = cognome;
        cliente.Email = email;
        cliente.Telefono = telefono;
        cliente.Indirizzo = indirizzo;
    }
    public async Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        Cliente? cliente = await ordiniDbContext.Clienti.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (cliente == null)
        {
            return;
        }
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
        return await ordiniDbContext.Ordini.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
    public async Task<List<Ordine>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.Ordini.ToListAsync(cancellationToken);
    }
    public async Task UpdateOrdineAsync(int id, decimal totale, int fk_cliente, CancellationToken cancellationToken = default)
    {
        Ordine? ordine = await ordiniDbContext.Ordini.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (ordine == null)
        {
            return;
        }
        ordine.Totale = totale;
        ordine.Fk_cliente = fk_cliente;
    }
    public async Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        Ordine? ordine = await ordiniDbContext.Ordini.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (ordine == null)
        {
            return;
        }
        ordiniDbContext.Ordini.Remove(ordine);
    }

    // OrdineProdotti
    public async Task<OrdineProdotti> AddProdottoToOrdineAsync(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = new OrdineProdotti
        {
            Fk_ordine = fk_ordine,
            Fk_prodotto = fk_prodotto,
            Quantita = quantita
        };
        await ordiniDbContext.OrdineProdotti.AddAsync(ordineProdotti, cancellationToken);
        return ordineProdotti;
    }
    public async Task<OrdineProdotti?> ReadOrdiniProdottiAsync(int id, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.OrdineProdotti.FirstOrDefaultAsync(op => op.Id == id, cancellationToken);
    }
    public async Task<List<OrdineProdotti>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default)
    {
        return await ordiniDbContext.OrdineProdotti.Where(op => op.Fk_ordine == fk_ordine).ToListAsync(cancellationToken);
    }
    public async Task UpdateOrdiniProdottiAsync(int id, int quantita, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = await ordiniDbContext.OrdineProdotti.FirstOrDefaultAsync(op => op.Id == id, cancellationToken);
        if (ordineProdotti == null)
        {
            throw new ArgumentException("OrdineProdotti non trovato.", nameof(id));
        }
        ordineProdotti.Quantita = quantita;
    }
    public async Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordineProdotti = await ordiniDbContext.OrdineProdotti.FirstOrDefaultAsync(op => op.Id == id, cancellationToken);
        if (ordineProdotti == null)
        {
            throw new ArgumentException("OrdineProdotti non trovato.", nameof(id));
        }
        ordiniDbContext.OrdineProdotti.Remove(ordineProdotti);
    }
}
