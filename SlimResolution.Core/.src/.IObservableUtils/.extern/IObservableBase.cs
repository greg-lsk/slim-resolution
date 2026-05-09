namespace SlimResolution.Core.IObservableUtils;

public interface IObservableBase<TObserver, TUnsubscriber>
{
    public TUnsubscriber Subscribe(TObserver observer);
}