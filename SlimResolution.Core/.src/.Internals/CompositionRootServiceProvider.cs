namespace SlimResolution.Core.Internals;

internal class CompositionRootServiceProvider : ICompositionRootServiceProvider
{
    public object Provider { get; }


    internal CompositionRootServiceProvider(object provider)
    {
        Provider = provider;
    }
}