using Ordini.ClientHttp.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordini.ClientHttp.DependencyInjection;

public static class OrdiniClientExtensions
{
    public static IServiceCollection AddOrdiniClient(this IServiceCollection services, IConfiguration configuration)
    {
        IConfiguration confSection = configuration.GetSection(OrdiniClientOptions.SectionName);
        OrdiniClientOptions options = confSection.Get<OrdiniClientOptions>() ?? new();

        services.AddHttpClient<IOrdiniClientHttp, OrdiniClientHttp>(o =>
        {
            o.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });
        return services;
    }
}
