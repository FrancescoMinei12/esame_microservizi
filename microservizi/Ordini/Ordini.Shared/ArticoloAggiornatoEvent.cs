using Newtonsoft.Json;
namespace Ordini.Shared;
public class ArticoloAggiornatoEvent
{
    [JsonProperty("id")]
    public int ArticoloId { get; set; }

    [JsonProperty("Prezzo")]
    public decimal NuovoPrezzo { get; set; }
}
