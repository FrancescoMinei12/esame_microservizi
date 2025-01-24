using Ordini.Business.Abstractions;
using Ordini.Repository.Abstraction;
using Ordini.Shared;
using Microsoft.Extensions.Logging;

namespace Ordini.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    // Clienti
    public async Task CreateClienteAsync(string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {
        await repository.CreateClienteAsync(nome, cognome, email, telefono, indirizzo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<ClienteDto?> GetClienteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await repository.ReadClienteAsync(id, cancellationToken);
        if (cliente == null) return null;
        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cognome = cliente.Cognome,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Indirizzo = cliente.Indirizzo
        };
    }
    public async Task<ClienteDto?> GetClienteByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var cliente = await repository.ReadClienteByEmailAsync(email, cancellationToken);
        if (cliente == null) return null;
        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Cognome = cliente.Cognome,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Indirizzo = cliente.Indirizzo
        };
    }
    public async Task<List<ClienteDto>> GetAllClientiAsync(CancellationToken cancellationToken = default)
    {
        var clienti = await repository.GetAllClientiAsync(cancellationToken);
        var clientiDto = clienti
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Cognome = c.Cognome,
                Email = c.Email,
                Telefono = c.Telefono,
                Indirizzo = c.Indirizzo
            }).ToList();
        return clientiDto;
    }
    public async Task UpdateClienteAsync(int id, string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default)
    {
        await repository.UpdateClienteAsync(id, nome, cognome, email, telefono, indirizzo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteClienteAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    // Ordini
    public async Task CreateOrdineAsync(int fk_cliente, decimal totale, CancellationToken cancellationToken = default)
    {
        await repository.CreateOrdineAsync(fk_cliente, totale, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<OrdineDto?> GetOrdineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordine = await repository.ReadOrdineAsync(id, cancellationToken);
        if (ordine == null) return null;
        return new OrdineDto
        {
            Id = ordine.Id,
            DataOrdine = ordine.DataOrdine,
            Totale = ordine.Totale,
            Fk_cliente = ordine.Fk_cliente
        };
    }
    public async Task<List<OrdineDto>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        var ordini = await repository.GetAllOrdiniAsync(cancellationToken);
        var ordiniDto = ordini
            .Select(o => new OrdineDto
            {
                Id = o.Id,
                DataOrdine = o.DataOrdine,
                Totale = o.Totale,
                Fk_cliente = o.Fk_cliente
            }).ToList();
        return ordiniDto;
    }
    public async Task UpdateOrdineAsync(int id, decimal totale, int fk_cliente, CancellationToken cancellationToken = default)
    {
        await repository.UpdateOrdineAsync(id, totale, fk_cliente, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.DeleteOrdineAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    // OrdiniProdotti
    public async Task AddOrdineProdottoAsync(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default)
    {
        await repository.AddProdottoToOrdineAsync(fk_ordine, fk_prodotto, quantita, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task<OrdineProdottiDto?> GetOrdineProdottoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordineProdotto = await repository.ReadOrdiniProdottiAsync(id, cancellationToken);
        if (ordineProdotto == null) return null;
        return new OrdineProdottiDto
        {
            Id = ordineProdotto.Id,
            Quantita = ordineProdotto.Quantita,
            Fk_ordine = ordineProdotto.Fk_ordine,
            Fk_prodotto = ordineProdotto.Fk_prodotto
        };
    }
    public async Task<List<OrdineProdottiDto>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default)
    {
        var prodotti = await repository.GetProdottiByOrdineAsync(fk_ordine, cancellationToken);
        var prodottiDto = prodotti
            .Select(p => new OrdineProdottiDto
            {
                Id = p.Id,
                Quantita = p.Quantita,
                Fk_ordine = p.Fk_ordine,
                Fk_prodotto = p.Fk_prodotto
            }).ToList();
        return prodottiDto;
    }
    public async Task UpdateOrdineProdottoAsync(int id, int quantita, decimal prezzoUnitario, CancellationToken cancellationToken = default)
    {
        await repository.UpdateOrdiniProdottiAsync(id, quantita, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.RemoveProdottoFromOrdineAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
