using Ordini.Business.Abstractions;
using Ordini.Repository.Abstraction;
using Ordini.Shared;
using Microsoft.Extensions.Logging;
using Pagamenti.Shared;
using Inventario.ClientHttp.Abstraction;
using Pagamenti.ClientHttp.Abstraction;
using Inventario.Shared;

namespace Ordini.Business;
public class Business(IRepository repository, ILogger<Business> logger, IInventarioClientHttp inventarioClientHttp, IClientHttp pagamentiClientHttp) : IBusiness
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
        return cliente.MapToDto();
    }
    public async Task<ClienteDto?> GetClienteByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var cliente = await repository.ReadClienteByEmailAsync(email, cancellationToken);
        if (cliente == null) return null;
        return cliente.MapToDto();
    }
    public async Task<List<ClienteDto>> GetAllClientiAsync(CancellationToken cancellationToken = default)
    {
        var clienti = await repository.GetAllClientiAsync(cancellationToken);
        return clienti
            .Select(c =>c.MapToDto())
            .ToList();
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

    public async Task<OrdineDto?> AggiornaTotaleOrdineAsync(int id, decimal nuovoTotale, CancellationToken cancellationToken = default)
    {
        var ordine = await repository.AggiornaTotaleOrdineAsync(id, nuovoTotale, cancellationToken);
        if (ordine == null) return null;
        return ordine.MapToDto();
    }
    public async Task CreateOrdineCompletoAsync(int fk_cliente, List<ProdottoQuantita> prodotti, int metodoPagamentoId, CancellationToken cancellationToken = default)
    {
        try
        {
            decimal totale = 0;
            foreach (var prodotto in prodotti)
            {
                logger.LogInformation($"Recupero articolo per prodotto ID {prodotto.ProdottoId}.");
                var articolo = await inventarioClientHttp.GetArticoloAsync(prodotto.ProdottoId, cancellationToken);
                if (articolo == null || articolo.QuantitaDisponibile < prodotto.Quantita)
                {
                    logger.LogError($"Prodotto con ID '{prodotto.ProdottoId}' non disponibile o quantità insufficiente.");
                    throw new Exception($"Prodotto con ID '{prodotto.ProdottoId}' non disponibile o quantità insufficiente.");
                }
                logger.LogInformation($"Articolo recuperato: {articolo.Nome}, Prezzo: {articolo.Prezzo}, Quantità disponibile: {articolo.QuantitaDisponibile}");
                totale += articolo.Prezzo * prodotto.Quantita;
            }

            logger.LogInformation($"Totale ordine calcolato: {totale}");
            var ordine = await repository.CreateOrdineAsync(fk_cliente, totale, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Ordine creato con ID {ordine.Id}");
            foreach (var prodotto in prodotti)
            {
                await repository.AddProdottoToOrdineAsync(ordine.Id, prodotto.ProdottoId, prodotto.Quantita, cancellationToken);
                await inventarioClientHttp.ScaricaQuantitaAsync(prodotto.ProdottoId, prodotto.Quantita, cancellationToken);
            }
            await repository.SaveChangesAsync(cancellationToken);

            var pagamento = new PagamentoDto
            {
                Importo = totale,
                DataPagamento = DateTime.Now,
                Fk_Ordine = ordine.Id,
                Fk_MetodoPagamento = metodoPagamentoId
            };

            logger.LogInformation("Invio pagamento: {@Pagamento}", pagamento);
            await pagamentiClientHttp.CreatePagamentoAsync(pagamento, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Errore durante la creazione dell'ordine.");
            throw;
        }
    }


    public async Task<OrdineDto?> GetOrdineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var ordine = await repository.ReadOrdineAsync(id, cancellationToken);
        if (ordine == null) return null;
        return ordine.MapToDto();
    }
    public async Task<List<OrdineDto>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        var ordini = await repository.GetAllOrdiniAsync(cancellationToken);
        return ordini
            .Select(o => o.MapToDto())
            .ToList();
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
        return ordineProdotto.MapToDto();
    }
    public async Task<List<OrdineProdottiDto>> GetProdottiByOrdineAsync(int fk_ordine, CancellationToken cancellationToken = default)
    {
        var prodotti = await repository.GetProdottiByOrdineAsync(fk_ordine, cancellationToken);
        return prodotti
            .Select(p => p.MapToDto())
            .ToList();
    }
    public async Task<List<ArticoloDto>> GetProdottiDetailsByOrdine(int fk_ordine, CancellationToken cancellationToken = default)
    {
        var prodotti = await GetProdottiByOrdineAsync(fk_ordine, cancellationToken);
        if (prodotti == null || prodotti.Count == 0)
           throw new Exception($"Nessun prodotto trovato per l'ordine con ID '{fk_ordine}'.");
        var dettagliProdotti = new List<ArticoloDto>();
        foreach (var articolo in prodotti)
        {
            var dettagliProdotto = await inventarioClientHttp.GetArticoloAsync(articolo.Fk_prodotto, cancellationToken);
            if (dettagliProdotto != null)
                dettagliProdotti.Add(dettagliProdotto);
        }
        return dettagliProdotti;
    }
    public async Task UpdateOrdineProdottoAsync(int id, int quantita, int fk_ordine, int fk_prodotto, CancellationToken cancellationToken = default)
    {
        await repository.UpdateOrdineProdottoAsync(id, quantita, fk_ordine, fk_prodotto, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
    public async Task RemoveProdottoFromOrdineAsync(int id, CancellationToken cancellationToken = default)
    {
        await repository.RemoveProdottoFromOrdineAsync(id, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
