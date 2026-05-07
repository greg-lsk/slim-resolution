using System;

using SlimResolution.Core.ServiceProviderAbstractions;
using SlimResolution.Core.ResolutionComposition.Internals;


namespace SlimResolution.Core.ExtensionHelpers;

public readonly struct ExtensionContext
{
    public static ExtensionContext Instance => new();


    public TResolved TryResolve<TResolved>(IComposer<TResolved> composer, object externalSource)
        where TResolved : struct
    {
        return composer switch
        {
            IFromSourceComposer<TResolved> validComposer => validComposer.Compose(ResolutionSource.Create(externalSource)),
            _ => throw new ArgumentException("Invalid Composer")
        };
    }
}