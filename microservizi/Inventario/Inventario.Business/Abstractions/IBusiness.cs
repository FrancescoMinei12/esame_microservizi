using Inventario.Shared;

namespace Inventario.Business.Abstractions;
public interface IBusiness
{
    // Articoli
    Task CreateArticoloAsync(string nome, string desc, decimal prezzo, int quantita, string SKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ScaricaQuantitaAsync(int id, int quantita, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
    Task<ArticoloDto?> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default);
    Task<List<ArticoloDto?>> GetCategoriaAsync(string categoria, CancellationToken cancellationToken = default);
    Task<List<ArticoloDto?>> ReadArticoloFornitore(int id_fornitore, CancellationToken cancellationToken = default);
    Task<List<ArticoloDto>> ReadAllArticoli(CancellationToken cancellationToken = default);
    Task UpdateArticoloAsync(int id, string nome, string descrizione, decimal prezzo, int quantitaDisponibile, string codiceSKU, string categoria, int fk_fornitore, CancellationToken cancellationToken = default);
    Task DeleteArticoloAsync(int id, CancellationToken cancellationToken = default);

    // Fornitori
    Task CreateFornitoreAsync(string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default);
    Task<FornitoreDto?> ReadFornitoreAsync(int id, CancellationToken cancellationToken = default);
    Task<List<FornitoreDto>> GetAllFornitoriAsync(CancellationToken cancellationToken = default);
    Task UpdateFornitoreAsync(int id, string nome, string indirizzo, string telefono, string email, CancellationToken cancellationToken = default);
    Task DeleteFornitoreAsync(int id, CancellationToken cancellationToken = default);
}
