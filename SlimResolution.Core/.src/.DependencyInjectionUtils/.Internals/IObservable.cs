namespace SlimResolution.Core.DependencyInjectionUtils.Internals;


public delegate void Unsubscribe();
public interface IObservable<out T>
{
    public Unsubscribe Subscribe(System.IObserver<T> observer);
}