using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core.Internals;

internal class Composer<T> : IComposer<T> where T : struct
{
    private readonly ResolutionSource _resolutionSource;

    internal IResolutionMetadata<T> Metadata { get; }
    public CreateResolutionSource ResolutionSourceFactory { get; }


    public Composer(IResolutionMetadata<T> metadata, ICompositionRootServiceProvider rootProvider)
    {
        var factory = SourceFactory.Instance.Factory;

        _resolutionSource = factory(rootProvider.Provider);
        
        ResolutionSourceFactory = factory;
        Metadata = metadata;
    }


    public T Compose() => Metadata.Materialize(_resolutionSource);
}