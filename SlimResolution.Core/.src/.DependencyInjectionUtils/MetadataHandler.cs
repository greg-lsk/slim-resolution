using System;
using System.Reflection;

using SlimResolution.Core.IObservableUtils;
using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public sealed class MetadataHandler : Observable<RegistrationInfo>
{
    private readonly string[] _metadataHostAssemblyNames;

    
    public MetadataHandler(string[] metadataHostAssemblyNames,
                           IObserverCollection<IObserver<RegistrationInfo>> observers) : base(observers)
    {
        _metadataHostAssemblyNames = metadataHostAssemblyNames;
    }
    public static MetadataHandler Create(string[] metadataHostAssemblyNames,
                                         IObserverCollection<IObserver<RegistrationInfo>> observers)
    {
        return new(metadataHostAssemblyNames, observers);
    }


    public void Run()
    {
        foreach (var assemblyName in _metadataHostAssemblyNames)
        {
            var assembly = Assembly.LoadFrom(assemblyName);

            Observers.OnEach(o =>
            {
                assembly.GetTypes()
                        .FilterByMetadata()
                        .OnEach(o.OnNext);
            });
        }
    }
}