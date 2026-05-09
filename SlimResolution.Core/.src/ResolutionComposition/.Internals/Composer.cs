using SlimResolution.Core.ServiceProviderAbstractions;


namespace SlimResolution.Core.ResolutionComposition.Internals;

internal class Composer<T> : IComposer<T>, IFromSourceComposer<T> where T : struct
{
    private readonly object _resolutionSource;
    private readonly ValidateResolutionSource _validateResolutionSource;

    private readonly IResolutionMetadata<T> _metadata;


    public Composer(object resolutionSource,
                    ValidateResolutionSource validateResolutionSource,
                    IResolutionMetadata<T> metadata)
    {
        _resolutionSource = resolutionSource;
        _validateResolutionSource = validateResolutionSource;

        _metadata = metadata;
    }


    T IComposer<T>.Compose() => _metadata.Materialize(ResolutionSource.Create(_resolutionSource));
    T IFromSourceComposer<T>.Compose(ResolutionSource source) => _metadata.Materialize(source);
}