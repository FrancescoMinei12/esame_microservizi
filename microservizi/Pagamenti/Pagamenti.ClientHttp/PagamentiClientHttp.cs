using Pagamenti.ClientHttp.Abstraction;
using Pagamenti.Shared;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Json;

namespace Pagamenti.ClientHttp;
public class PagamentiClientHttp(HttpClient httpClient) : IClientHttp
{
    public async Task<string?> CreatePagamentoAsync(PagamentoDto pagamento, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsync($"/Pagamenti/CreatePagamento", JsonContent.Create(pagamento), cancellationToken);
        return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync(cancellationToken: cancellationToken);
    }
    public async Task<PagamentoDto?> GetPagamentoAsync(int id, CancellationToken cancellationToken)
    {
        var queryString = QueryString.Create(new Dictionary<string, string>() {
            { "id", id.ToString(CultureInfo.InvariantCulture) }
        });
        var response = await httpClient.GetAsync($"/Pagamenti/GetPagamentoById{queryString}", cancellationToken);
        return await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<PagamentoDto>(cancellationToken: cancellationToken);
    }
}