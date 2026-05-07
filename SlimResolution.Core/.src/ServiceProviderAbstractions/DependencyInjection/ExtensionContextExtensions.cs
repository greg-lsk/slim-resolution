using System;

using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.ServiceProviderAbstractions.Internals;


namespace SlimResolution.Core.ServiceProviderAbstractions.DependencyInjection;

public static class ExtensionContextExtensions
{
    public static void RegisterResolutionProvider<TProvider>(this ExtensionContext context,
                                                             Action<Type, Func<TProvider, object>> register)
        where TProvider : notnull
    {
        register(typeof(ICompositionRootServiceProvider), provider => new CompositionRootServiceProvider(provider));
    }
}