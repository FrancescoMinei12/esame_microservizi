using Inventario.Repository.Model;

namespace Inventario.Repository.Abstraction;

public interface IRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Articoli
    Task<Articolo> CreateArticoloAsync(string nome, string descrizione, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default);
    Task<Articolo?> ModificaPrezzoArticoloAsync(int id, int nuovoPrezzo, CancellationToken cancellationToken = default);
    Task<Articolo?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<Articolo?> ReadArticoloAsync(string codiceSKU, CancellationToken cancellationToken = default);
    Task<List<Articolo>> ReadArticoloCategoria(string categoria, CancellationToken cancellationToken = default);
    Task<List<Articolo?>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default);
    Task<List<Articolo>> ReadAllArticoli();
    Task UpdateArticoloAsync(int id, string nome, string descrizione, decimal prezzo, int quantitaDisponibile, string codiceSKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default);
    Task DeleteArticoloAsync(int id, CancellationToken cancellationToken = default);

    // Fornitori
    Task<Fornitore> CreateFornitoreAsync(string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default);
    Task<Fornitore?> ReadFornitoreAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Fornitore>> GetAllFornitoriAsync(CancellationToken cancellationToken = default);
    Task UpdateFornitoreAsync(int id, string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default);
    Task DeleteFornitoreAsync(int id, CancellationToken cancellationToken = default);
}
