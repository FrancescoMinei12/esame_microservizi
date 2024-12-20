using Inventario.Repository.Model;

namespace Inventario.Repository.Abstraction;

public interface IRepository
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task CreateArticoloAsync(string nome, string desc, decimal prezzo, int quantita, string SKU, string categoria, CancellationToken cancellationToken = default);
    Task<Articolo?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default);
}
