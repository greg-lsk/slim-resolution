using System;

using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Extensions.Microsoft.Internals;


namespace SlimResolution.Extensions.Microsoft;

public static class RegistrationHelper
{
    public static TService GenericResolution<TService>(IResolutionContext context) 
        where TService : notnull
    {
        return (context as ResolutionContext).ProviderSelector().GetRequiredService<TService>();
    }

    internal static ResolutionContext CreateContext(Func<IServiceProvider> providerSelector)
    {
        return new(providerSelector);
    }
}