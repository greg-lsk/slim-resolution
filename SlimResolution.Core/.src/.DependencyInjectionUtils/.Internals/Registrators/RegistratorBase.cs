using System;

using SlimResolution.Core.IObservableUtils;
using SlimResolution.Core.MetadataTypeDiscovery.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils.Internals;

internal abstract class RegistratorBase<TProvider> : IObserver<RegistrationInfo>
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


    public void RegisterTo(Observable<RegistrationInfo> observable)
    {
        UnsubscribeCallback = observable.Subscribe(this);
    }

    public abstract void OnNext(RegistrationInfo reflectionInfo);

    public void OnCompleted() => UnsubscribeCallback?.Invoke();
    
    public void OnError(Exception error) { }
}