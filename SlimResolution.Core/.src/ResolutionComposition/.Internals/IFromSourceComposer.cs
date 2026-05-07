using SlimResolution.Core.ServiceProviderAbstractions;


namespace SlimResolution.Core.ResolutionComposition.Internals;

internal interface IFromSourceComposer<T> where T : struct
{
    internal T Compose(ResolutionSource source);
}