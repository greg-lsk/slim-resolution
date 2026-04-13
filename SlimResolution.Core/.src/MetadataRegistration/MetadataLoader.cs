using System.Reflection;
using SlimResolution.Core.MetadataRegistration.Internals;


namespace SlimResolution.Core.MetadataRegistration;

public class MetadataLoader
{
    private readonly string[] _metadataHostAssemblyNames;


    private MetadataLoader(string[] metadataHostAssemblyNames)
    {
        _metadataHostAssemblyNames = metadataHostAssemblyNames;
    }
    public static MetadataLoader Create(string[] metadataHostAssemblyNames)
    {
        return new(metadataHostAssemblyNames);
    }


    public void OnEach(MetadataInfoHandler handler)
    {
        foreach (var assemblyName in _metadataHostAssemblyNames)
        {
            var assembly = Assembly.LoadFrom(assemblyName);

            assembly.GetTypes()
                    .FilterByMetadata()
                    .OnEach(handler);
        }
    }
}