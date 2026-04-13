using Microsoft.Extensions.DependencyInjection;
    
using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.MetadataRegistration;

using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class Registration
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services)
    {
        var metadataLoader = MetadataLoader.Instance;
        var extensionContext = ExtensionContext.Instance;

        services.AddSingleton(typeof(IComposer<>), extensionContext.GetComposerType())
                .AddSingleton<IResolutionContext, ResolutionContext>(provider =>
                {
                    return new(() => provider);
                });

        metadataLoader.OnEach((in m) =>
        {
            m.GetResolutionProperties()
             .RunRegistration
             (
                in m,
                (i, f) => services.AddSingleton(i, provider => f()),
                (s, c) => (c as ResolutionContext).ProviderSelector().GetService(s)
             );
        });

        return services;
    }
}