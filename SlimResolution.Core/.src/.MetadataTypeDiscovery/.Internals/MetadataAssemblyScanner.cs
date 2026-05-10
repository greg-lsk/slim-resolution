using System.Reflection;


namespace SlimResolution.Core.MetadataTypeDiscovery.Internals;

internal class MetadataAssemblyScanner : IMetadataTypeLocator
{
    private readonly string[] _metadataHostAssemblyNames;
    private event HandleRegistrationInfo? HandleRegistrationInfo;


    internal MetadataAssemblyScanner(string[] metadataHostAssemblyNames)
    {
        _metadataHostAssemblyNames = metadataHostAssemblyNames;
    }


    internal void AddHandler(HandleRegistrationInfo handleRegistrationInfo) => HandleRegistrationInfo += handleRegistrationInfo;

    public void Run()
    {
        foreach (var assemblyName in _metadataHostAssemblyNames)
        {
            var assembly = Assembly.LoadFrom(assemblyName);

            assembly.GetTypes()
                    .FilterByMetadata()
                    .OnEach(m => HandleRegistrationInfo?.Invoke(m));
        }
    }
}