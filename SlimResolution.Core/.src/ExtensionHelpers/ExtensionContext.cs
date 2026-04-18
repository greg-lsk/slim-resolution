using System;
using SlimResolution.Core.Internals;
using SlimResolution.Core.ErrorHandling.StaticThrowHelpers;


namespace SlimResolution.Core.ExtensionHelpers;

public readonly struct ExtensionContext
{
    public static ExtensionContext Instance => new();


    public void RegisterIComposer(Action<Type, Type> register)
    {
        register(typeof(IComposer<>), typeof(Composer<>));
    }

    public TResolved Matarialize<TResolved>(IComposer<TResolved> composer, ResolutionSource source)
        where TResolved : struct
    {
        InvalidArgumentException.ThrowIfNotDefaultComposer(composer);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
//Unreachable if composer is invalid — ThrowIfNotDefaultComposer already throws, so the cast is safe.
        return (composer as Composer<TResolved>).Metadata.Materialize(source);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}