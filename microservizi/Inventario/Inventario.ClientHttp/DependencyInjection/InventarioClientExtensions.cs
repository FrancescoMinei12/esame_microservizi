using Inventario.ClientHttp;
using Inventario.ClientHttp.Abstraction;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class InventarioClientExtensions
{
    public static IServiceCollection AddInventarioClient(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection confSection = configuration.GetSection(InventarioClientOptions.SectionName);
        InventarioClientOptions options = confSection.Get<InventarioClientOptions>() ?? new();

        services.AddHttpClient<IInventarioClientHttp, InventarioClientHttp>(obj =>
        {
            obj.BaseAddress = new Uri(options.BaseAddress);
        }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });
        return services;
    }
}