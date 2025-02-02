namespace Microsoft.Extensions.DependencyInjection;
public class InventarioClientOptions
{
    public const string SectionName = "InventarioClientHttp";
    public string BaseAddress { get; set; } = "";
}