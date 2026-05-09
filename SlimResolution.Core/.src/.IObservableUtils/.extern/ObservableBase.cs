namespace SlimResolution.Core.IObservableUtils;

public abstract class ObservableBase<TObserver, TUnsubscriber> : IObservableBase<TObserver, TUnsubscriber>
{
    protected IObserverCollection<TObserver> Observers;


    protected ObservableBase(IObserverCollection<TObserver> observers)
    {
        Observers = observers;
    }


    public abstract TUnsubscriber Subscribe(TObserver observer);
}