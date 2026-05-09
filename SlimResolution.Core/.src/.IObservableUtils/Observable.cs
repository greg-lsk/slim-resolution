using System;

namespace SlimResolution.Core.IObservableUtils;


public delegate void Unsubscribe();

public abstract class Observable<T> : ObservableBase<IObserver<T>, Unsubscribe>
{
    protected Observable(IObserverCollection<IObserver<T>> observers) : base(observers) { }


    public override Unsubscribe Subscribe(IObserver<T> observer)
    {
        Observers.Add(observer);
        return () => Observers.Remove(observer);
    }
}