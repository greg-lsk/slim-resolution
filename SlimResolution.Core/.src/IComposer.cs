using System;


namespace SlimResolution.Core;

public interface IComposer<T> where T : struct
{
    public Func<object, ResolutionSource> ResolutionSourceFactory { get; }

    public T Compose();
}