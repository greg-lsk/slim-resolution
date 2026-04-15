using System;
using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ErrorHandling;


namespace SlimResolution.Extensions.MicrosoftDI.Internals;

internal class ResolutionContext : IResolutionContext
{
    public LinkToken LinkToken { get; }
    internal Func<IServiceProvider> ProviderSelector { get; }

    
    internal ResolutionContext(Func<IServiceProvider> providerSelector, LinkToken linkToken)
    {
        ProviderSelector = providerSelector;
        LinkToken = linkToken;
    }
    internal static ResolutionContext Create(Func<IServiceProvider> providerSelector)
    {
        var token = providerSelector().GetRequiredService<IResolutionContext>().LinkToken;

        return new(providerSelector, token);
    }
}