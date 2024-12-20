using Inventario.Business.Abstractions;
using Inventario.Repository.Abstraction;
using Inventario.Shared;
using Microsoft.Extensions.Logging;

namespace Inventario.Business;

public class Business(IRepository repository, ILogger<Business> logger) : IBusiness
{
    public async Task CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default)
    {
        await repository.CreateArticoloAsync(articoloDto.Nome, articoloDto.Descrizione, articoloDto.Prezzo, articoloDto.QuantitaDisponibile, articoloDto.CodiceSKU, articoloDto.Categoria, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ArticoloDto> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default)
    {
        // fixare con client
        return await Task.FromResult<ArticoloDto>(new ArticoloDto());
    }

    public async Task<ArticoloDto?> ReadArticoloAsync(int id, CancellationToken cancellationToken = default)
    {
        var articolo = await repository.ReadArticoloAsync(id, cancellationToken);
        if (articolo == null)
            return null;

        return new ArticoloDto
        {
            Nome = articolo.Nome,
            Descrizione = articolo.Descrizione,
            Prezzo = articolo.Prezzo,
            QuantitaDisponibile = articolo.QuantitaDisponibile,
            CodiceSKU = articolo.CodiceSKU,
            Categoria = articolo.Categoria,
        };
    }
}
