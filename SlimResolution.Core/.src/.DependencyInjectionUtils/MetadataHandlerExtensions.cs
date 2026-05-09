using SlimResolution.Core.DependencyInjectionUtils.Internals;


namespace SlimResolution.Core.DependencyInjectionUtils;

public static class MetadataHandlerExtensions
{
    public static MetadataHandler InitializeRegistrators<TProvider>(this MetadataHandler metadataHandler,
                                                                    ResolveServiceFromType<TProvider> resolveFromType,
                                                                    RegisterService<TProvider> register) 
        where TProvider : notnull
    {
        var metadataRegistrator = new MetadataRegistrator<TProvider>(resolveFromType, register);
        var composerRegistrator = new ComposerRegistrator<TProvider>(resolveFromType, register);

        metadataRegistrator.RegisterTo(metadataHandler);
        composerRegistrator.RegisterTo(metadataHandler);

        return metadataHandler;
    }
}