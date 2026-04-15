using SlimResolution.Core.ErrorHandling;


namespace SlimResolution.Core;

public interface IResolutionMetadata<T> where T : struct 
{
    public LinkToken LinkToken { get; }
    public T Materialize(IResolutionContext context);
}