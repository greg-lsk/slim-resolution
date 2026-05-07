using System;
using Microsoft.Extensions.DependencyInjection;
    
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.MetadataRegistration;
using SlimResolution.Core.ResolutionSourceProcessing.DependencyInjection;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services,
                                                       string[] metadataHostAssemblyNames)
    {
        var extensionContext = ExtensionContext.Instance;
        var metadataLoader = MetadataLoader.Create(metadataHostAssemblyNames);

        extensionContext.RegisterIComposer((t1, t2) => services.AddSingleton(t1, t2));
        extensionContext.RegisterSourceProcessingEssentials((t1, t2) => services.AddTransient(t1, t2));
        extensionContext.RegisterResolutionProvider<IServiceProvider>((t, f) => services.AddSingleton(t, f));

        metadataLoader.OnEach((in m) =>
        {
            m.GetResolutionProperties()
             .RunRegistration
             (
                in m,
                o => o is IServiceProvider,
                (s, o) => (o as IServiceProvider).GetService(s),
                (i, f) => services.AddSingleton(i, provider => f(provider))
             );
        });

        return services;
    }
}