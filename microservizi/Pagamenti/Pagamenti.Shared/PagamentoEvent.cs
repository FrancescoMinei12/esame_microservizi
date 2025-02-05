using Newtonsoft.Json;
namespace Pagamenti.Shared;
public class PagamentoEvent
{
    [JsonProperty("OrdineId")]
    public int OrdineId { get; set; }
    [JsonProperty("NuovoTotale")]
    public decimal NuovoTotale { get; set; }
}
