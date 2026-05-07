namespace SlimResolution.Core.ServiceProviderAbstractions;

public readonly struct ResolutionSource
{
    private readonly object _source;


    private ResolutionSource(object source)
    {
        _source = source;
    }
}