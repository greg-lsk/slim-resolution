namespace SlimResolution.Core.ResolutionSourceProcessing.Internals;

internal interface IDelegateFactory
{
    internal CreateResolutionSource TryConstructCreationDelegate(IReflectionContext context);
    internal AccessRootServiceProvider TryConstructAccessDelegate(IReflectionContext context);
}