using SlimResolution.Core.IObservableUtils;
using SlimResolution.Core.DependencyInjectionUtils;
using SlimResolution.Core.DependencyInjectionUtils.Internals;
using SlimResolution.Core.MetadataTypeDiscovery.Internals;


namespace SlimResolution.Core.MetadataTypeDiscovery;

public static class Factory
{
    public static IMetadataTypeLocator Create(string[] assemblyNames) => new MetadataAssemblyScanner(assemblyNames);

    public static IMetadataTypeLocator Configure<TProvider>(this IMetadataTypeLocator locator,
                                                            ResolveServiceFromType<TProvider> resolveFromType,
                                                            RegisterService<TProvider> register)
        where TProvider : notnull
    {
        var metadataRegistrator = new MetadataRegistrator<TProvider>(resolveFromType, register);
        var composerRegistrator = new ComposerRegistrator<TProvider>(resolveFromType, register);

        var registratorCollection = new ObserverCollection<TProvider>();
        registratorCollection.Add(metadataRegistrator);
        registratorCollection.Add(composerRegistrator);

        var registratorMonitor = new RegistratorMonitor(registratorCollection);

        (locator as MetadataAssemblyScanner).AddHandler(registratorMonitor.Handle);

        return locator;
    }
}
