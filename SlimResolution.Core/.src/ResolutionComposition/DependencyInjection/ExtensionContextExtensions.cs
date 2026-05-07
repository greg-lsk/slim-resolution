using System;

using SlimResolution.Core.ExtensionHelpers;
using SlimResolution.Core.ResolutionComposition.Internals;


namespace SlimResolution.Core.ResolutionComposition.DependencyInjection;

public static class ExtensionContextExtensions
{
    public static void RegisterIComposer(this ExtensionContext context, Action<Type, Type> register)
    {
        register(typeof(IComposer<>), typeof(Composer<>));
    }
}