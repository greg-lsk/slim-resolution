using SlimResolution.Core;
using SlimResolution.Core.MetadataRegistration;

using Microsoft.Extensions.DependencyInjection;


namespace SlimResolution.Extensions.MicrosoftDI.Internals;

internal class ServiceResolver : IServiceResolver
{
    private ServiceResolver() { }
    internal static ServiceResolver Instance => new();

    public TService Resolve<TService>(IResolutionContext context) where TService : notnull
    {
        return (context as ResolutionContext).ProviderSelector().GetRequiredService<TService>();
    }
}