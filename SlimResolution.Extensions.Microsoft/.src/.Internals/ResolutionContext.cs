using System;
using SlimResolution.Core;


namespace SlimResolution.Extensions.MicrosoftDI.Internals;

internal class ResolutionContext : IResolutionContext
{
    internal Func<IServiceProvider> ProviderSelector { get; }


    public ResolutionContext(Func<IServiceProvider> providerSelector)
    {
        ProviderSelector = providerSelector;
    }
    internal static ResolutionContext Create(Func<IServiceProvider> providerSelector)
    {
        return new(providerSelector);
    }
}