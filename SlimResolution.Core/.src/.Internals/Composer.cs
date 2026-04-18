using System;
using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core.Internals;

internal class Composer<T> : IComposer<T> where T : struct
{
    private readonly ResolutionSource _resolutionSource;

    internal IResolutionMetadata<T> Metadata { get; }
    public Func<object, ResolutionSource> ResolutionSourceFactory { get; }


    public Composer(IResolutionMetadata<T> metadata, ICompositionRootProvider rootProvider)
    {
        var factory = new SourceFactory().Factory;

        _resolutionSource = factory(rootProvider.Provider);
        
        ResolutionSourceFactory = factory;
        Metadata = metadata;
    }


    public T Compose() => Metadata.Materialize(_resolutionSource);
}


public interface ICompositionRootProvider
{
    public object Provider { get; }
}
public class CompositionRootProvider : ICompositionRootProvider
{
    public object Provider { get; }


    public CompositionRootProvider(object provider)
    {
        Provider = provider;
    }
}