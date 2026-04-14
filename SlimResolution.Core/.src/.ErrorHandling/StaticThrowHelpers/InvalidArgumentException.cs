using SlimResolution.Core.Internals;


namespace SlimResolution.Core.ErrorHandling.StaticThrowHelpers;

internal static class InvalidArgumentException
{
    internal static void ThrowIfNotDefaultComposer<TTarget>(IComposer<TTarget> composer) where TTarget : struct
    {
        if (composer is Composer<TTarget>) return;

        throw new System.ArgumentException(
            $"\n{nameof(composer)} must not be a custom implementation of: '{typeof(IComposer<TTarget>)}';" +
            $"\nActual type was: '{composer.GetType()}'.\n");
    }
}