using Microsoft.Extensions.DependencyInjection;
    
using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.MetadataRegistration;

using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services,
                                                       string[] metadataHostAssemblyNames)
    {
        var extensionContext = ExtensionContext.Instance;
        var metadataLoader = MetadataLoader.Create(metadataHostAssemblyNames);

        var linkToken = LinkToken.Create<ResolutionContext, IResolutionMetadata<int>>();


        services.AddSingleton(typeof(IComposer<>), extensionContext.GetComposerType())
                .AddSingleton<IResolutionContext, ResolutionContext>(provider =>
                {
                    return new(() => provider, linkToken);
                });

        metadataLoader.OnEach((in m) =>
        {
            m.GetResolutionProperties()
             .RunRegistration
             (
                in m,
                linkToken,
                (i, f) => services.AddSingleton(i, provider => f()),
                (s, c) => (c as ResolutionContext).ProviderSelector().GetService(s)
             );
        });

        return services;
    }
}