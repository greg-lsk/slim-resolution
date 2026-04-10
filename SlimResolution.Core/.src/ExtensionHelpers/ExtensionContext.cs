using System;
using SlimResolution.Core.Internals;


namespace SlimResolution.Core.ExtensionHelpers;

public readonly struct ExtensionContext
{
    public static ExtensionContext Instance => new();


    public Type GetComposerType()
    {
        return typeof(Composer<>);
    }

    public TResolved Matarialize<TResolved>(IComposer<TResolved> composer, IResolutionContext context)
        where TResolved : struct
    {
        return (composer as Composer<TResolved>).Metadata.Materialize(context);
    }
}