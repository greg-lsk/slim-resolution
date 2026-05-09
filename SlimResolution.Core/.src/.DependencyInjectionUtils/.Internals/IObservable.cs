namespace SlimResolution.Core.DependencyInjectionUtils.Internals;


public delegate void Unsubscibe();
public interface IObservable<out T>
{
    public Unsubscibe Subscribe(System.IObserver<T> observer);
}