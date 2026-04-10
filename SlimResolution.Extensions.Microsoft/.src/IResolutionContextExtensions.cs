using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Extensions.Microsoft.Internals;


namespace SlimResolution.Extensions.Microsoft;

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