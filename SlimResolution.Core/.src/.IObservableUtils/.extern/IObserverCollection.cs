using System;


namespace SlimResolution.Core.IObservableUtils;

public interface IObserverCollection<TObserver>
{
    public void Add(TObserver observer);
    public void Remove(TObserver observer);
    public void OnEach(Action<TObserver> action);

    public void Clear();
}