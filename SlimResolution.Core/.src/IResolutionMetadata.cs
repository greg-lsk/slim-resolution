namespace SlimResolution.Core;

public interface IResolutionMetadata<T> where T : struct 
{
    public T Materialize(ResolutionSource source);

    public bool IsLinkedTo(ResolutionSource source);
}