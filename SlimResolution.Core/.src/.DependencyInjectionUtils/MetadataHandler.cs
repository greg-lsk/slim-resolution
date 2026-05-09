using System.Reflection;

using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public class MetadataHandler : IObservable<RegistrationInfo>
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

    public Unsubscribe Subscribe(System.IObserver<RegistrationInfo> observer)
    {
        OnMetadataTypeHit += observer.OnNext;

        return () => OnMetadataTypeHit -= observer.OnNext;
    }
}