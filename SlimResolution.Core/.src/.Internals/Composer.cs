namespace SlimResolution.Core.Internals;

internal class Composer<T> : IComposer<T> where T : struct
{
    internal readonly IResolutionMetadata<T> Metadata;
    internal readonly IResolutionContext Context;


    public Composer(IResolutionMetadata<T> metadata, IResolutionContext context)
    {
        Metadata = metadata;
        Context = context;
    }


    public T Compose() => Metadata.Materialize(Context);
}