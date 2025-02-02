namespace Pagamenti.ClientHttp.DependencyInjection;
public class PagamentiClientOptions
{
    public const string SectionName = "PagamentiClientHttp";
    public string BaseAddress { get; set; } = string.Empty;
}