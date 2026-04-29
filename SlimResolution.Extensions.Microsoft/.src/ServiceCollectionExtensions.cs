using System;
using Microsoft.Extensions.DependencyInjection;
    
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.Internals;
using SlimResolution.Core.MetadataRegistration;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services,
                                                       string[] metadataHostAssemblyNames)
    {
        var extensionContext = ExtensionContext.Instance;
        var metadataLoader = MetadataLoader.Create(metadataHostAssemblyNames);

        services.AddSingleton<ICompositionRootProvider, CompositionRootProvider>(provider => new(provider));

        extensionContext.RegisterIComposer((t1, t2) => services.AddSingleton(t1, t2));

        metadataLoader.OnEach((in m) =>
        {
            m.GetResolutionProperties()
             .RunRegistration
             (
                in m,
                o => o is IServiceProvider,
                (s, o) => (o as IServiceProvider).GetService(s),
                (i, f) => services.AddSingleton(i, provider => f())
             );
        });

        return services;
    }
}