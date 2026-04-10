using System;
using SlimResolution.Core;


namespace SlimResolution.Extensions.Microsoft.Internals;

internal class ResolutionContext : IResolutionContext
{
    internal Func<IServiceProvider> ProviderSelector { get; }


    public ResolutionContext(Func<IServiceProvider> providerSelector)
    {
        ProviderSelector = providerSelector;
    }
}