namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal abstract class RegistratorBase<TProvider> : System.IObserver<MetadataInfo>
    where TProvider : notnull
{
    protected Unsubscribe? UnsubscibeCallback { get; private set; }

    protected ResolveServiceFromType<TProvider> ResolveFromType { get; }
    protected RegisterService<TProvider> Register { get; }


    protected RegistratorBase(ResolveServiceFromType<TProvider> resolveMetadataDependency,
                              RegisterService<TProvider> registerMetadata)
    {
        ResolveFromType = resolveMetadataDependency;
        Register = registerMetadata;
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