using Ordini.Repository.Model;

namespace Ordini.Repository.Abstraction;

public interface IRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    // Clienti
    Task<Cliente> CreateClienteAsync(string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default);
    Task<Cliente?> ReadClienteAsync(int id, CancellationToken cancellationToken = default);
    Task<Cliente?> ReadClienteByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<Cliente>> GetAllClientiAsync(CancellationToken cancellationToken = default);
    Task UpdateClienteAsync(int id, string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default);
    Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default);

    // Ordini
    Task<Ordine> CreateOrdineAsync(int fk_cliente, decimal totale, CancellationToken cancellationToken = default);
    Task<Ordine?> ReadOrdineAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Ordine>> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
    Task UpdateOrdineAsync(int id, decimal totale, int fk_cliente, CancellationToken cancellationToken = default);
    Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default);

    // OrdineProdotti
    Task<OrdineProdotti> AddProdottoToOrdineAsync(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default);
    Task<OrdineProdotti?> ReadOrdiniProdottiAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OrdineProdotti>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default);
    Task UpdateOrdineProdottoAsync(int id, int quantita, int fk_ordine, int fk_prodotto, CancellationToken cancellationToken = default);
    Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default);
}
