using Pagamenti.ClientHttp;
using Pagamenti.ClientHttp.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pagamenti.ClientHttp.DependencyInjection;

public static class PagamentiClientExtensions
{
    public static IServiceCollection AddPagamentiClient(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection confSection = configuration.GetSection(PagamentiClientOptions.SectionName);
        PagamentiClientOptions options = confSection.Get<PagamentiClientOptions>() ?? new();

        services.AddHttpClient<IClientHttp, PagamentiClientHttp>(o =>
        {
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });
        return services;
    }
}