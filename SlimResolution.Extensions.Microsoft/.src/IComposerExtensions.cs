using Microsoft.Extensions.DependencyInjection;

using SlimResolution.Core;
using SlimResolution.Core.ExtensionHelpers;


namespace SlimResolution.Extensions.MicrosoftDI;

public static class IComposerExtensions
{
    public static TResolved ComposeFor<TResolved>(this IComposer<TResolved> composer, IServiceScope scope)
        where TResolved : struct
    {
        return ExtensionContext.Instance.TryResolve(composer, scope.ServiceProvider);
    }   
}