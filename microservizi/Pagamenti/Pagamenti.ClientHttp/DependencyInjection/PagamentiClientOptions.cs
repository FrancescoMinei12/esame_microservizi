namespace Pagamenti.ClientHttp.DependencyInjection;

internal class PagamentiClientOptions
{
    public const string SectionName = "PagamentiClientHttp";
    public string BaseAddress { get; set; } = string.Empty;
}
