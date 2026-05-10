using System;
using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.MetadataTypeDiscovery;
using SlimResolution.Core.ResolutionSourceProcessing.DependencyInjection;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services,
                                                       string[] metadataHostAssemblyNames)
    {
        var extensionContext = ExtensionContext.Instance;
        extensionContext.RegisterSourceProcessingEssentials((t1, t2) => services.AddTransient(t1, t2));

        Factory.Create(metadataHostAssemblyNames)
               .Configure<IServiceProvider>
               (
                    (p, s) => p.GetService(s),
                    (i, f) => services.AddSingleton(i, provider => f(provider))
               )
               .Run();

        return services;
    }
}