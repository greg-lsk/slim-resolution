using SlimResolution.Core.ServiceProviderAbstractions;
using SlimResolution.Core.ServiceProviderAbstractions.Internals;


namespace SlimResolution.Core.ResolutionComposition.Internals;

internal class Composer<T> : IComposer<T>, IFromSourceComposer<T> where T : struct
{
    private readonly ResolutionSource _resolutionSource;
    private readonly IResolutionMetadata<T> _metadata;


    public Composer(IResolutionMetadata<T> metadata,
                    ICompositionRootServiceProvider rootProvider)
    {
        _metadata = metadata;
        _resolutionSource = ResolutionSource.Create(rootProvider.Provider);
    }


    T IComposer<T>.Compose() => _metadata.Materialize(_resolutionSource);
    T IFromSourceComposer<T>.Compose(ResolutionSource source) => _metadata.Materialize(source);
}