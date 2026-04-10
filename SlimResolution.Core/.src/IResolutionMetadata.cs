namespace SlimResolution.Core;

public interface IResolutionMetadata<T> where T : struct 
{ 
    public T Materialize(IResolutionContext context);
}