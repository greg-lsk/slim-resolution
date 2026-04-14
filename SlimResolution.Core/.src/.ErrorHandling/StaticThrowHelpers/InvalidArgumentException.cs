using SlimResolution.Core.Internals;


namespace SlimResolution.Core.ErrorHandling.StaticThrowHelpers;

public static class InvalidArgumentException
{
    internal static void ThrowIfNotDefaultComposer<TTarget>(IComposer<TTarget> composer) where TTarget : struct
    {
        if (composer is Composer<TTarget>) return;

        throw new System.ArgumentException(
            $"\n{nameof(composer)} must not be a custom implementation of: '{typeof(IComposer<TTarget>)}';" +
            $"\nActual type was: '{composer.GetType()}'.\n");
    }

    public static void ThrowIfNotBound<T, TMetadata>(IResolutionMetadata<T> metadata) 
        where T : struct
        where TMetadata : class, IResolutionMetadata<T>
    {
        if (metadata is TMetadata) return;

        throw new System.ArgumentException(
            $"\n{nameof(metadata)} is not the bound metadata to: '{typeof(T)}';" +
            $"\nBound metadata type is: '{typeof(TMetadata)}'." +
            $"\nActual type was: '{metadata.GetType()}'.\n");
    }
}