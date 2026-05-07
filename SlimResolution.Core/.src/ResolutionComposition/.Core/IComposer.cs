using SlimResolution.Core.ResolutionSourceProcessing;


namespace SlimResolution.Core;

public interface IComposer<T> where T : struct
{
    public CreateResolutionSource ResolutionSourceFactory { get; }

    public T Compose();
}