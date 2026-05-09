using System;

using SlimResolution.Core.DependencyInjectionUtils;
using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.IObservableUtils;

public class ObserverCollection<TProvider> : IObserverCollection<IObserver<RegistrationInfo>>
    where TProvider : notnull
{
    private readonly IObserver<RegistrationInfo>?[] _observers = new IObserver<RegistrationInfo>?[2];


    public static ObserverCollection<TProvider> Instance => new();


    public void Add(IObserver<RegistrationInfo> observer)
    {
        if (observer is MetadataRegistrator<TProvider>) _observers[0] = observer;
        if (observer is ComposerRegistrator<TProvider>) _observers[1] = observer;
    }

    public void Clear() 
    {
        for (int i = 0; i < _observers.Length; i++) _observers[i] = null;
    }

    public void OnEach(Action<IObserver<RegistrationInfo>> action)
    {
        for (int i = 0; i < _observers.Length; i++)
        {
            var observer = _observers[i];
            if (observer is not null) action(observer);
        }
    }

    public void Remove(IObserver<RegistrationInfo> observer)
    {
        if (observer is MetadataRegistrator<TProvider>) _observers[0] = null;
        if (observer is ComposerRegistrator<TProvider>) _observers[1] = null;
    }
}