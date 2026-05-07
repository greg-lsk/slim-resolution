namespace SlimResolution.Core.ServiceProviderAbstractions;

public readonly struct ResolutionSource
{
    private readonly object _source;


    private ResolutionSource(object source)
    {
        _source = source;
    }
    internal static ResolutionSource Create(object source) => new(source);
}