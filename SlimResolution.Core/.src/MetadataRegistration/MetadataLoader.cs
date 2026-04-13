using SlimResolution.Core.MetadataRegistration.Internals;


namespace SlimResolution.Core.MetadataRegistration;

public abstract class MetadataLoader
{
    public abstract void OnEach(MetadataInfoHandler handler);

    public static MetadataLoader Instance => new MetadataAssembyLoader();
}