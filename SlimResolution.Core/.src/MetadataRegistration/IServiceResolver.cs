namespace SlimResolution.Core.MetadataRegistration;

public interface IServiceResolver
{
    public TService Resolve<TService>(IResolutionContext context) where TService : notnull;
}