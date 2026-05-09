using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public static class MetadataHandlerExtensions
{
    public static MetadataHandler InitializeRegistrators<TProvider>(this MetadataHandler metadataHandler,
                                                                    ResolveMetadataDependency resolveMetadataDependency,
                                                                    RegisterMetadata registerMetadata) 
        where TProvider : notnull
    {
        var metadataRegistrator = new MetadataRegistrator(resolveMetadataDependency,registerMetadata);
        var composerRegistrator = new ComposerRegistrator<TProvider>(resolveMetadataDependency, registerMetadata);

        metadataRegistrator.RegisterTo(metadataHandler);
        composerRegistrator.RegisterTo(metadataHandler);

        return metadataHandler;
    }
}