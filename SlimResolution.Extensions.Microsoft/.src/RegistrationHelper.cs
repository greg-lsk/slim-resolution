using System;

using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Extensions.MicrosoftDI.Internals;


namespace SlimResolution.Extensions.MicrosoftDI;

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