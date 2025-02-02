using Ordini.Repository.Model;
using Ordini.Shared;

namespace Ordini.Business.Abstractions;
public interface IBusiness
{
    // Clienti
    Task CreateClienteAsync(string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default);
    Task<ClienteDto?> GetClienteByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ClienteDto?> GetClienteByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<ClienteDto>> GetAllClientiAsync(CancellationToken cancellationToken = default);
    Task UpdateClienteAsync(int id, string nome, string cognome, string email, string telefono, string indirizzo, CancellationToken cancellationToken = default);
    Task DeleteClienteAsync(int id, CancellationToken cancellationToken = default);

    // Ordini
    Task CreateOrdineAsync(int fk_cliente, decimal totale, CancellationToken cancellationToken = default);
    Task<OrdineDto?> AggiornaTotaleOrdineAsync(int id, decimal nuovoTotale, CancellationToken cancellationToken = default);
    Task CreateOrdineCompletoAsync(int fk_cliente, List<ProdottoQuantita> prodotti, int metodoPagamentoId, CancellationToken cancellationToken = default);
    Task<OrdineDto?> GetOrdineByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OrdineDto>> GetAllOrdiniAsync(CancellationToken cancellationToken = default);
    Task UpdateOrdineAsync(int id, decimal totale, int fk_cliente, CancellationToken cancellationToken = default);
    Task DeleteOrdineAsync(int id, CancellationToken cancellationToken = default);

    // OrdineProdotti
    Task AddOrdineProdottoAsync(int fk_ordine, int fk_prodotto, int quantita, CancellationToken cancellationToken = default);
    Task<OrdineProdottiDto?> GetOrdineProdottoByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<OrdineProdottiDto>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default);
    Task UpdateOrdineProdottoAsync(int id, int quantita, int fk_ordine, int fk_prodotto, CancellationToken cancellationToken = default);
    Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default);
}