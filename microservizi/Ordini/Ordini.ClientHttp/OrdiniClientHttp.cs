using Ordini.ClientHttp.Abstraction;
using Ordini.Shared;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;
using System.Net.Http;

namespace Ordini.ClientHttp;

public class OrdiniClientHttp(HttpClient httpClient) : IOrdiniClientHttp
{
    public async Task<string?> CreateOrdineAsync(OrdineDto ordineDto, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsync($"Ordini/CreateOrdine", JsonContent.Create(ordineDto), cancellationToken);
        return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);
    }
    public async Task<OrdineDto?> GetOrdineByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var queryString = QueryString.Create(new Dictionary<string, string?>()
        {
            { "id",id.ToString(CultureInfo.InvariantCulture)}
        });
        var response = await httpClient.GetAsync($"Ordini/GetOrdineById{queryString}", cancellationToken);
        return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<OrdineDto?>(cancellationToken: cancellationToken);
    }
    public async Task<List<OrdineDto>> GetAllOrdiniAsync(CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync("/Ordini/GetAllOrdini", cancellationToken);
        var ordiniDto = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<List<OrdineDto>>(cancellationToken: cancellationToken);
        return ordiniDto ?? new List<OrdineDto>();
    }
}
