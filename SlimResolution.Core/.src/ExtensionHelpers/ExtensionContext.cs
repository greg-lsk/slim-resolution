using System;
using SlimResolution.Core.Internals;
using SlimResolution.Core.ErrorHandling.StaticThrowHelpers;


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
        InvalidArgumentException.ThrowIfNotDefaultComposer(composer);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
//Unreachable if composer is invalid — ThrowIfNotDefaultComposer already throws, so the cast is safe.
        return (composer as Composer<TResolved>).Metadata.Materialize(context);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}