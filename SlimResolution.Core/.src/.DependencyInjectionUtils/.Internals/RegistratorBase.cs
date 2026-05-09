namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal abstract class RegistratorBase : System.IObserver<MetadataInfo>
{
    protected Unsubscibe? UnsubscibeCallback { get; private set; }

    protected ResolveMetadataDependency ResolveMetadataDependency { get; }
    protected RegisterMetadata RegisterMetadata { get; }


    protected RegistratorBase(ResolveMetadataDependency resolveMetadataDependency,
                              RegisterMetadata registerMetadata)
    {
        ResolveMetadataDependency = resolveMetadataDependency;
        RegisterMetadata = registerMetadata;
    }


    public void RegisterTo(IObservable<MetadataInfo> observable)
    {
        UnsubscibeCallback = observable.Subscribe(this);
    }

    public abstract void OnNext(MetadataInfo metadataInfo);

    public void OnCompleted() 
    {
        if (UnsubscibeCallback is not null) UnsubscibeCallback();
    }
    public void OnError(System.Exception error) { }
}