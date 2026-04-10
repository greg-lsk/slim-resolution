using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class Registration
{
    public static IServiceCollection AddSlimResolution(this IServiceCollection services)
    {
        return services.AddSingleton(typeof(IComposer<>), ExtensionContext.Instance.GetComposerType())
                       .AddSingleton<IResolutionContext, ResolutionContext>(provider =>
                       {
                           return new(() => provider);
                       });
        //scan assembly for metadata and add them as well
    }
}