using System.Reflection;

using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public class MetadataHandler : IObservable<MetadataInfo>
{
    private readonly string[] _metadataHostAssemblyNames;

    public event HandleMetadataInfo? OnMetadataTypeHit;

    
    private MetadataHandler(string[] metadataHostAssemblyNames)
    {
        _metadataHostAssemblyNames = metadataHostAssemblyNames;
    }
    public static MetadataHandler Create(string[] metadataHostAssemblyNames)
    {
        return new(metadataHostAssemblyNames);
    }


    public void Run()
    {
        foreach (var assemblyName in _metadataHostAssemblyNames)
        {
            var assembly = Assembly.LoadFrom(assemblyName);

            if (OnMetadataTypeHit is null) break;

            assembly.GetTypes()
                    .FilterByMetadata()
                    .OnEach(OnMetadataTypeHit);
        }
    }

    public Unsubscibe Subscribe(System.IObserver<MetadataInfo> observer)
    {
        OnMetadataTypeHit += observer.OnNext;

        return () => OnMetadataTypeHit -= observer.OnNext;
    }
}