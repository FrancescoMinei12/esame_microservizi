using Inventario.ClientHttp.Abstraction;
using Inventario.Shared;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Inventario.ClientHttp;

public class ClientHttp(HttpClient httpClient) : IClientHttp
{
    public async Task<string?> CreateArticoloAsync(ArticoloDto articoloDto, CancellationToken cancellationToken = default)
    {
        var res = await httpClient.PostAsync($"/Articolo/CreateArticolo", JsonContent.Create(articoloDto), cancellationToken);
        return await res.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
    }

    public async Task<ArticoloDto?> GetSkuAsync(string CodiceSKU, CancellationToken cancellationToken = default)
    {
        var queryString = QueryString.Create(new Dictionary<string, string?>()
        {
            {"CodiceSKU", CodiceSKU.ToString(CultureInfo.InvariantCulture)}
        });
        var res = await httpClient.GetAsync($"/Articolo/GetSku{queryString}", cancellationToken);
        return await res.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ArticoloDto?>(cancellationToken: cancellationToken);
    }

    public async Task<ArticoloDto?> GetArticoloAsync(int id, CancellationToken cancellationToken = default)
    {
        var queryString = QueryString.Create(new Dictionary<string, string?>()
        {
            {"id", id.ToString(CultureInfo.InvariantCulture)}
        });
        var res = await httpClient.GetAsync($"/Articolo/ReadArticolo{queryString}", cancellationToken);
        return await res.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ArticoloDto?>(cancellationToken: cancellationToken);
    }
}
