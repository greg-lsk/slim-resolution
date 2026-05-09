namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal abstract class RegistratorBase<TProvider> : System.IObserver<RegistrationInfo>
    where TProvider : notnull
{
    protected Unsubscribe? UnsubscribeCallback { get; private set; }

    protected ResolveServiceFromType<TProvider> ResolveFromType { get; }
    protected RegisterService<TProvider> Register { get; }


    protected RegistratorBase(ResolveServiceFromType<TProvider> resolveFromType,
                              RegisterService<TProvider> register)
    {
        ResolveFromType = resolveFromType;
        Register = register;
    }


    public void RegisterTo(IObservable<RegistrationInfo> observable)
    {
        UnsubscribeCallback = observable.Subscribe(this);
    }

    public abstract void OnNext(RegistrationInfo reflectionInfo);

    public void OnCompleted() => UnsubscribeCallback?.Invoke();
    
    public void OnError(System.Exception error) { }
}