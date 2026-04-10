using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class IResolutionContextExtensions
{
    public static IServiceCollection AddResolutionContext(this IServiceCollection services)
    {
        services.AddSingleton<IResolutionContext, ResolutionContext>(provider =>
        {
            return new(() => provider);
        });

        return services;
    }
}